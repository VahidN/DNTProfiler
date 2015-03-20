using System.Globalization;
using System.Web;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Common.WebToolkit
{
    public static class HttpContextUtils
    {
        private static string _aspnetTemporaryFilesFolder;

        public static string GetAspnetTemporaryFilesFolder()
        {
            if (!string.IsNullOrWhiteSpace(_aspnetTemporaryFilesFolder))
                return _aspnetTemporaryFilesFolder;

            if (!IsInWeb)
                return string.Empty;

            try
            {
                return _aspnetTemporaryFilesFolder = HttpRuntime.CodegenDir;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetCurrentUrl()
        {
            try
            {
                return !IsInWeb ? string.Empty : HttpContext.Current.Request.Url.AbsoluteUri.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int? GetHttpContextCurrentId()
        {
            return !IsInWeb ? (int?)null : UniqueIdExtensions<HttpContext>.GetUniqueId(HttpContext.Current).ToInt(); ;
        }

        public static bool IsInWeb
        {
            get { return HttpContext.Current != null; }
        }
    }
}