using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace DNTProfiler.Services.Help
{
    public class ApiDocumentationRepository
    {
        public static ApiEndPoint Get(string apiName, HttpConfiguration config)
        {
            return getApiEndPoint(apiName, config);
        }

        public static List<ApiEndPoint> Get(HttpConfiguration config)
        {
            var apiDescriptions = new ApiExplorer(config).ApiDescriptions;
            var controllers = apiDescriptions
                .GroupBy(x => x.ActionDescriptor.ControllerDescriptor.ControllerName)
                .Select(x => x.First().ActionDescriptor.ControllerDescriptor.ControllerName)
                .ToList();

            var apiEndPoints = new List<ApiEndPoint>();

            foreach (var controller in controllers)
            {
                apiEndPoints.Add(getApiEndPoint(controller, config));
            }

            return apiEndPoints;
        }

        static ApiEndPoint getApiEndPoint(string controller, HttpConfiguration config)
        {
            string ctrl = controller;
            var apis = new ApiExplorer(config)
             .ApiDescriptions
             .Where(x => x.ActionDescriptor.ControllerDescriptor.ControllerName == ctrl).ToList();

            List<ApiEndPointDetail> apiEndPointDetails = null;

            if (apis.ToList().Count > 0)
            {

                apiEndPointDetails = new List<ApiEndPointDetail>();
                foreach (var api in apis)
                {
                    apiEndPointDetails.Add(getApiEndPointDetail(api));
                }
            }
            else
            {
                controller = string.Format("The {0} api does not exist.", controller);
            }
            return new ApiEndPoint(controller, apiEndPointDetails);
        }

        static ApiEndPointDetail getApiEndPointDetail(ApiDescription api)
        {
            if (api.ParameterDescriptions.Any())
            {
                var parameters = new List<ApiEndPointParameter>();
                foreach (var parameter in api.ParameterDescriptions)
                {
                    parameters.
                    Add(new ApiEndPointParameter(parameter.Name, parameter.Documentation, parameter.Source.ToString()));
                }
                return new ApiEndPointDetail(api.RelativePath, api.Documentation, api.HttpMethod.Method, parameters);
            }
            return new ApiEndPointDetail(api.RelativePath, api.Documentation, api.HttpMethod.Method);
        }
    }
}