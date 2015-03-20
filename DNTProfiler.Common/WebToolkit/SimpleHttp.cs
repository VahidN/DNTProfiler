using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace DNTProfiler.Common.WebToolkit
{
    public class SimpleHttp : ISimpleHttp
    {
        public HttpStatusCode PostAsJson(string url, object data, JsonSerializerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");

            return PostAsJson(new Uri(url), data, settings);
        }

        public HttpStatusCode PostAsJson(Uri url, object data, JsonSerializerSettings settings)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            var postRequest = (HttpWebRequest)WebRequest.Create(url);
            postRequest.Method = "POST";
            postRequest.UserAgent = "SimpleHttp/1.0";
            postRequest.ContentType = "application/json; charset=utf-8";

            using (var stream = new StreamWriter(postRequest.GetRequestStream()))
            {
                var serializer = JsonSerializer.Create(settings);
                using (var writer = new JsonTextWriter(stream))
                {
                    serializer.Serialize(writer, data);
                    writer.Flush();
                }
            }

            using (var response = (HttpWebResponse)postRequest.GetResponse())
            {
                return response.StatusCode;
            }
        }
    }
}