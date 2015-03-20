using System.Collections.Generic;

namespace DNTProfiler.Services.Help
{
    public class ApiEndPointDetail
    {
        public string RelativePath { get; private set; }
        public string Documentation { get; private set; }
        public string Method { get; private set; }
        public List<ApiEndPointParameter> Parameters { get; private set; }

        public ApiEndPointDetail(string relativePath, string documentation, string method,
            List<ApiEndPointParameter> parameters)
            : this(relativePath, documentation, method)
        {
            Parameters = parameters;
        }

        public ApiEndPointDetail(string relativePath, string documentation, string method)
        {
            RelativePath = relativePath;
            Documentation = documentation;
            Method = method;
        }
    }
}