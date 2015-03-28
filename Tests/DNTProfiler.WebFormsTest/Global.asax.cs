using System;
using System.Data.Entity;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using StructureMap.Web.Pipeline;
using DNTProfiler.WebFormsTest.IoCConfig;

namespace DNTProfiler.WebFormsTest
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SampleContext, Configuration>());
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var list = ObjectFactory.Container.GetInstance<IProductService>().GetAllProducts();
            if (list != null)
            {
                // to test AuthenticateRequest
            }
        }
    }
}