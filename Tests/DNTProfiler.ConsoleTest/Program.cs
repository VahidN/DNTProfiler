using System;
using System.Collections.Generic;
using System.Data.Entity;
using DNTProfiler.ServiceLayer;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;
using StructureMap;
using StructureMap.Web.Pipeline;
using StructureMap.Web;

namespace DNTProfiler.ConsoleTest
{
    class Program
    {
        private static IContainer _container;

        private static void add2ProductsAnd1Category()
        {
            Console.WriteLine("method1");

            var rnd = new Random();

            using (var container = _container.GetNestedContainer())
            {
                var uow = container.GetInstance<IUnitOfWork>();
                var categoryService = container.GetInstance<ICategoryService>();

                var product1 = new Product { Name = "P100" + rnd.Next(2000), Price = 100 };
                var product2 = new Product { Name = "P200" + rnd.Next(2000), Price = 200 };
                var category1 = new Category
                {
                    Name = "Cat100" + rnd.Next(2000),
                    Title = "Title100",
                    Products = new List<Product> { product1, product2 }
                };
                categoryService.AddNewCategory(category1);
                uow.SaveAllChanges();
            }
        }

        private static void add2ProductsAnd1CategoryWithDispose()
        {
            Console.WriteLine("method2");

            var rnd = new Random();

            var uow = _container.GetInstance<IUnitOfWork>();
            var categoryService = _container.GetInstance<ICategoryService>();

            var product1 = new Product { Name = "P100" + rnd.Next(2000), Price = 100 };
            var product2 = new Product { Name = "P200" + rnd.Next(2000), Price = 200 };
            var category1 = new Category
            {
                Name = "Cat100" + rnd.Next(2000),
                Title = "Title100",
                Products = new List<Product> { product1, product2 }
            };
            categoryService.AddNewCategory(category1);
            uow.SaveAllChanges();

            new HybridLifecycle().FindCache(null).DisposeAndClear();
            //((IDisposable)uow).Dispose();
        }

        private static void issueIncorrectLazyLoadingmethod4()
        {
            var categoryService = _container.GetInstance<ICategoryService>();
            var list = categoryService.IssueIncorrectLazyLoading();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void issueNavigationalCount()
        {
            var categoryService = _container.GetInstance<ICategoryService>();
            Console.WriteLine("FirstCategoryProductsCount: {0}", categoryService.GetFirstCategoryProductsCount());
        }

        static void Main(string[] args)
        {
            startDb();

            _container = new Container(x =>
            {
                x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<SampleContext>();
                x.For<ICategoryService>().Use<EfCategoryService>();
            });

            add2ProductsAnd1Category();
            add2ProductsAnd1CategoryWithDispose();
            method3UnDisposedConnection();
            issueIncorrectLazyLoadingmethod4();
            issueNavigationalCount();

            Console.WriteLine("Press a key to terminate...");
            Console.ReadKey();
        }

        private static void method3UnDisposedConnection()
        {
            Console.WriteLine("method3UnDisposedConnection");

            var categoryService = _container.GetInstance<ICategoryService>();

            var items = categoryService.GetAllCategories();
            foreach (var category in items)
            {
                Console.WriteLine(category.Name);
            }
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