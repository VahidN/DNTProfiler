using System;
using System.Data.Entity;
using DNTProfiler.TestEFContext.DataLayer;
using Nito.AsyncEx;

namespace DNTProfiler.ConsoleAppDotNet45
{
    class Program
    {
        static void Main(string[] args)
        {
            //DNTProfiler.Common.Logger.CallingMethod.WontExcludeTypes = true;

            startDb();

            AsyncContext.Run(async () =>
            {
                await TestAsync.RunCommands();
            });

            Console.WriteLine("Press a key to terminate...");
            Console.ReadKey();
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