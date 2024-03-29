﻿using GeneralizableMoebooruAPI.Bases;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneralizableMoebooruAPI.Features
{
    public class AccountManager : FeatureBase
    {
        public AccountManager(APIWrapperOption option) : base(option)
        {

        }

        public async ValueTask<bool> LoginAsync(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(Option.PasswordSalts))
                throw new Exception("option PasswordSalts is empty");

            var user = new UserInfo() { Name = name };

            var cookie_container = new CookieContainer();
            var url = $"{Option.ApiBaseUrl}user/authenticate";
            var response = await HttpRequest.CreateRequestAsync(url, async req =>
            {
                req.Method = "POST";
                req.CookieContainer = cookie_container;
                req.ContentType = "application/x-www-form-urlencoded";

                var csrf_token = WebUtility.UrlEncode(await GetCSRFTokenAsync(cookie_container));
                var body = $"authenticity_token={csrf_token}&url=&user%5Bname%5D={name}&user%5Bpassword%5D={password}&commit=Login";

                Log.Debug($"login req url: {url}");
                Log.Debug($"login req body: {body}");

                using var req_writer = new StreamWriter(await req.GetRequestStreamAsync());
                await req_writer.WriteAsync(body);
                await req_writer.FlushAsync();
            });

            var cookies = cookie_container.GetCookies(response.ResponseUri).OfType<Cookie>().ToArray();

            using var reader = new StreamReader(response.GetResponseStream());
            var content = await reader.ReadToEndAsync();

            foreach (var cookie in cookies)
            {
                if (cookie.Name == "pass_hash")
                {
                    user.PasswordHash = cookie.Value;
                    Option.CurrentUser = user;
                    Log.Debug($"success.");

                    return true;
                }
            }

            Log.Debug($"failed , try to call LoginByHash().");
            return await LoginByHashAsync(name, password);
        }

        public async ValueTask<bool> LoginByHashAsync(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(Option.PasswordSalts))
                throw new Exception("option PasswordSalts is empty");

            var buffer = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(Option.PasswordSalts.Replace("your-password", password)));
            var password_hash = string.Join("", buffer.Select(x => x.ToString("X2"))).ToLower();

            var url = $"{Option.ApiBaseUrl}user/home?" + $"login={WebUtility.UrlEncode(name)}&password_hash={password_hash}";
            Log.Debug($"login full url:{url}");

            var response = await HttpRequest.CreateRequestAsync(url, req =>
            {
                req.Method = "GET";
            });

            using var reader = new StreamReader(response.GetResponseStream());
            if ((await reader.ReadToEndAsync()).Contains(name))
            {
                Option.CurrentUser = new UserInfo() { Name = name, PasswordHash = password_hash };
                Log.Debug($"success.");
                return true;
            }

            Log.Debug($"failed.");
            return false;
        }

        public Task LogoutAsync()
        {
            Option.CurrentUser = null;
            return Task.CompletedTask;
        }

        private async ValueTask<string> GetCSRFTokenAsync(CookieContainer container)
        {
            var req = await HttpRequest.CreateRequestAsync($"{Option.ApiBaseUrl}user/login", req => req.CookieContainer = container);
            var reader = new StreamReader(req.GetResponseStream());

            /*
             <meta name="csrf-token" content="2s3jOIwFfoOjCxchwh3U06H126ca3Fog7mmRM5AMKyqNKR7c3nBxOAfXEBTB4TBzBMxHbxDnhJhzb+4eEgr/UA==" />
             */
            var text = await reader.ReadToEndAsync();
            var token = Regex.Match(text, @"<meta\s+name=""csrf-token""\s+content=""(.+?)""\s+/>")?.Groups[1].Value;
            token = string.IsNullOrWhiteSpace(token) ? Regex.Match(text, @"<meta\s+content=""(.+?)""\s+name=""csrf-token""\s*/>")?.Groups[1].Value : token;

            return string.IsNullOrWhiteSpace(token) ? throw new Exception("无法获取CSRF令牌") : token;
        }
    }
}