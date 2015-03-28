using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Common.WebToolkit
{
    public static class HttpContextUtils
    {
        public static HashSet<string> DynamicFilesExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".aspx", ".asax", ".asp", ".ashx", ".asmx", ".axd", ".master", ".svc", ".php" ,
            ".php3" , ".php4", ".ph3", ".ph4", ".php4", ".ph5", ".sphp", ".cfm", ".ps", ".stm",
            ".htaccess", ".htpasswd", ".php5", ".phtml", ".cgi", ".pl", ".plx", ".py", ".rb", ".sh", ".jsp",
            ".cshtml", ".vbhtml", ".swf" , ".xap", ".asptxt"
        };

        private static readonly object _lockObject = new object();
        private static string _aspnetTemporaryFilesFolder;
        private static IList<string> _bundles;

        public static bool IsInWeb
        {
            get { return HttpContext.Current != null; }
        }

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

        /// <summary>
        /// Gets System.Web.Optimization.BundleTable.Bundles
        /// </summary>
        public static IList<string> GetBundles()
        {
            if (_bundles != null)
            {
                return _bundles;
            }

            lock (_lockObject)
            {
                _bundles = new List<string>();

                var type = Type.GetType("System.Web.Optimization.BundleTable, System.Web.Optimization");
                if (type == null) return _bundles;

                var bundles = type.GetProperty("Bundles", BindingFlags.Static | BindingFlags.Public);
                if (bundles == null) return _bundles;

                var bundleItems = bundles.GetValue(bundles, null) as IEnumerable;
                if (bundleItems == null) return _bundles;

                foreach (var item in bundleItems)
                {
                    var propertyInfo = item.GetType().GetProperty("Path");
                    var path = propertyInfo.GetValue(item, null) as string;
                    if (path == null) continue;
                    _bundles.Add(path.TrimStart('~'));
                }
                return _bundles;
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

        public static bool IsStaticFile()
        {
            try
            {
                if (!IsInWeb)
                {
                    return false;
                }

                string[] reservedPaths =
                {
                    "/WebResource.axd",
                    "/__browserLink"
                };

                var request = HttpContext.Current.Request;
                var rawUrl = request.RawUrl;

                if (reservedPaths.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                if (GetBundles().Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                var physicalPath = request.PhysicalPath;
                if (string.IsNullOrWhiteSpace(physicalPath) ||
                    File.GetAttributes(physicalPath).HasFlag(FileAttributes.Directory))
                {
                    return false;
                }

                var extension = Path.GetExtension(physicalPath);
                return !DynamicFilesExtensions.Contains(extension);
            }
            catch
            {
                return false;
            }
        }
    }
}