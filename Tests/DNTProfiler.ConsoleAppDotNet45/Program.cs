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
            startDb();

            AsyncContext.Run(async () =>
            {
                var ctx = new SampleContext();
                var list = await ctx.Products.ToListAsync();

                foreach (var product in list)
                {
                    Console.WriteLine(product.Name);
                }
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