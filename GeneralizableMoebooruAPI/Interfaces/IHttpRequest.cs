using System;
using System.Net;
using System.Threading.Tasks;

namespace GeneralizableMoebooruAPI.Interfaces
{
    public interface IHttpRequest
    {
        ValueTask<HttpWebResponse> CreateRequestAsync(string url);
        ValueTask<HttpWebResponse> CreateRequestAsync(string url, Func<HttpWebRequest,Task> custom = default);
        ValueTask<HttpWebResponse> CreateRequestAsync(string url, Action<HttpWebRequest> custom = default);
    }

    public class HttpRequestDefaultImplement : IHttpRequest
    {
        public static IHttpRequest Default { get; } = new HttpRequestDefaultImplement();

        public ValueTask<HttpWebResponse> CreateRequestAsync(string url) => CreateRequestAsync(url, _ => Task.CompletedTask);

        public ValueTask<HttpWebResponse> CreateRequestAsync(string url, Action<HttpWebRequest> custom = null) => CreateRequestAsync(url, req =>
        {
            custom(req);
            return Task.CompletedTask;
        });

        public async ValueTask<HttpWebResponse> CreateRequestAsync(string url, Func<HttpWebRequest, Task> custom = null)
        {
            var req = WebRequest.Create(url);
            req.Method = "GET";

            await custom?.Invoke(req as HttpWebRequest);

            return await req.GetResponseAsync() as HttpWebResponse;
        }
    }
}