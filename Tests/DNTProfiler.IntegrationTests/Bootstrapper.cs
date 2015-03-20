using System.Data.Entity;
using System.Threading;
using DNTProfiler.TestEFContext.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class Bootstrapper
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            startDb();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Let the DNTProfiler.Core.CommandsTransmitter to complete its job.
            Thread.Sleep(1000 * 60 * 5);
        }

        private static void startDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SampleContext, Configuration>());
            using (var ctx = new SampleContext())
            {
                ctx.Database.Initialize(force: true);
            }
        }
    }
}