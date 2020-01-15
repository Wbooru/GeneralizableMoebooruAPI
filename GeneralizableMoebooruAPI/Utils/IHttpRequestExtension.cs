using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GeneralizableMoebooruAPI.Utils
{
    public static class IHttpRequestExtension
    {
        public static string GetString(this WebResponse response)
        {
            using var reader = new StreamReader(response.GetResponseStream());

            return reader.ReadToEnd();
        }

        public static T GetJsonContainer<T>(this WebResponse response) where T : JContainer
        {
            return JsonConvert.DeserializeObject(response.GetString()) as T;
        }

        public static JObject GetJsonObject(this WebResponse response) => GetJsonContainer<JObject>(response);
    }
}
