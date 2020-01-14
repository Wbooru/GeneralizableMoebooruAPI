using System;
using System.Net;

namespace GeneralizableMoebooruAPI.Interfaces
{
    public interface IHttpRequest
    {
        HttpWebResponse CreateRequest(string url, Action<HttpWebRequest> custom = default);
    }

    public class HttpRequestDefaultImplement : IHttpRequest
    {
        public static IHttpRequest Default { get; } = new HttpRequestDefaultImplement();

        public HttpWebResponse CreateRequest(string url, Action<HttpWebRequest> custom = null)
        {
            var req = WebRequest.Create(url);
            req.Method = "GET";

            custom?.Invoke(req as HttpWebRequest);

            return req.GetResponse() as HttpWebResponse;
        }
    }
}