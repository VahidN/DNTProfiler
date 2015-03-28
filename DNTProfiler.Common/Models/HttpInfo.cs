using DNTProfiler.Common.Toolkit;
using DNTProfiler.Common.WebToolkit;

namespace DNTProfiler.Common.Models
{
    public class HttpInfo
    {
        public HttpInfo()
        {
            Url = HttpContextUtils.GetCurrentUrl();
            UrlHash = getUrlHash();
            HttpContextCurrentId = HttpContextUtils.GetHttpContextCurrentId();
            IsStaticFile = HttpContextUtils.IsStaticFile();
        }

        public int? HttpContextCurrentId { set; get; }

        public bool IsStaticFile { set; get; }

        public string Url { set; get; }

        public string UrlHash { set; get; }

        private string getUrlHash()
        {
            return string.IsNullOrWhiteSpace(Url) ? "" : Url.ToLowerInvariant().ComputeHash();
        }
    }
}