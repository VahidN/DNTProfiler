using System;
using System.Net;
using Newtonsoft.Json;

namespace DNTProfiler.Common.WebToolkit
{
    public interface ISimpleHttp
    {
        HttpStatusCode PostAsJson(string url, object data, JsonSerializerSettings settings);
        HttpStatusCode PostAsJson(Uri url, object data, JsonSerializerSettings settings);
    }
}