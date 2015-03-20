using System.Web;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.WebFormsTest.IoCConfig;

namespace DNTProfiler.WebFormsTest
{
    public class TestHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var sum = service.GetTotalPriceSumInt();

            context.Response.ContentType = "application/javascript";
            context.Response.Write("alert('Hello from TestHandler.ashx -> Sum: '+ " + sum + ");");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}