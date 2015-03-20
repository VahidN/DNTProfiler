using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using DNTProfiler.Services.Help;

namespace DNTProfiler.Services
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HelpController : ApiController
    {
        public List<ApiEndPoint> Get()
        {
            return ApiDocumentationRepository.Get(ControllerContext.Configuration);
        }

        public ApiEndPoint Get(string api)
        {
            return ApiDocumentationRepository.Get(api, ControllerContext.Configuration);
        }
    }
}