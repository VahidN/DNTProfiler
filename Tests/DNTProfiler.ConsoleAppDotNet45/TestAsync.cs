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
    }
}