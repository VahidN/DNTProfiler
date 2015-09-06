using System;
using System.Data.Entity;
using System.Threading.Tasks;
using DNTProfiler.TestEFContext.DataLayer;

namespace DNTProfiler.ConsoleAppDotNet45
{
    public static class TestAsync
    {
        public static async Task RunCommands()
        {
            await runAsyncMethods();
            await runFirstOrDefaultAsync();
            await runFindAsync();
        }

        private static async Task runAsyncMethods()
        {
            using (var ctx = new SampleContext())
            {
                var list = await ctx.Products.ToListAsync();
                foreach (var product in list)
                {
                    Console.WriteLine(product.Name);
                }

                var product1 = await ctx.Products.FirstOrDefaultAsync(product => product.Name.StartsWith("p"));
                if (product1 != null)
                {
                    Console.WriteLine(product1.Name);
                }
            }
        }

        private static async Task runFirstOrDefaultAsync()
        {
            using (var ctx = new SampleContext())
            {
                var product1 = await ctx.Products.FirstOrDefaultAsync(product => product.Id == 1);
                if (product1 != null)
                {
                    Console.WriteLine(product1.Name);
                }
            }
        }

        private static async Task runFindAsync()
        {
            using (var ctx = new SampleContext())
            {
                var product1 = await ctx.Products.FindAsync(1);
                if (product1 != null)
                {
                    Console.WriteLine(product1.Name);
                }
            }
        }
    }
}