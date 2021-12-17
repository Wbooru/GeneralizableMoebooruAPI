using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeneralizableMoebooruAPI
{
    public static class Utils
    {
        public static async ValueTask<string> GetStringAsync(this WebResponse response)
        {
            using var reader = new StreamReader(response.GetResponseStream());

            return await reader.ReadToEndAsync();
        }

        public static async ValueTask<T> GetJsonContainerAsync<T>(this WebResponse response) where T : JContainer
        {
            return JsonConvert.DeserializeObject(await response.GetStringAsync()) as T;
        }

        public static ValueTask<JObject> GetJsonObjectAsync(this WebResponse response) => GetJsonContainerAsync<JObject>(response);
    }
}
