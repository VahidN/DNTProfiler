using System;
using System.Web;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.WebFormsTest.IoCConfig;

namespace DNTProfiler.WebFormsTest
{
    public class TestModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var sum = service.GetTotalPriceSumInt();

            var app = sender as HttpApplication;
            if (app != null && !app.Request.RawUrl.EndsWith(".ashx", StringComparison.OrdinalIgnoreCase))
            {
                app.Context.Response.Write("<div align='center'>Hello from TestModule->EndRequest(" + sum + ")</div>");
            }
        }
    }
}