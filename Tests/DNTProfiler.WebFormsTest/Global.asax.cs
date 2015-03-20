using System;
using System.Data.Entity;
using DNTProfiler.TestEFContext.DataLayer;
using StructureMap.Web.Pipeline;

namespace DNTProfiler.WebFormsTest
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SampleContext, Configuration>());
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}