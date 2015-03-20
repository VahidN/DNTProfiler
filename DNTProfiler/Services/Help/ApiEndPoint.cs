using System.Collections.Generic;

namespace DNTProfiler.Services.Help
{
    public class ApiEndPoint
    {
        public string Name { get; private set; }
        public List<ApiEndPointDetail> ApiEndPointDetails { get; private set; }

        public ApiEndPoint(string name, List<ApiEndPointDetail> apiEndPointDetails)
        {
            Name = name;
            ApiEndPointDetails = apiEndPointDetails;
        }

    }
}