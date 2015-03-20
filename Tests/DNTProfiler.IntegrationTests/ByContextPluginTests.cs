using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class ByContextPluginTests
    {
        [ClassInitialize]
        [TestMethod]
        public static void ShouldAdd3ProductsAnd1Category(TestContext context)
        {
            var rnd = new Random();
            var product1 = new Product { Name = "P100" + rnd.Next(2000), Price = 100 };
            var product2 = new Product { Name = "P200" + rnd.Next(2000), Price = 200 };
            var product3 = new Product { Name = "P300" + rnd.Next(2000), Price = 300 };
            var category1 = new Category
            {
                Name = "Cat100" + rnd.Next(2000),
                Title = "Title100",
                Products = new List<Product> { product1, product2, product3 }
            };

            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var uow = container.GetInstance<IUnitOfWork>();
                var categoryService = container.GetInstance<ICategoryService>();
                categoryService.AddNewCategory(category1);
                uow.SaveAllChanges();
            }

            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var category = categoryService.FindCategory(category1.Id);

                Assert.IsNotNull(category);
            }
        }

        [TestMethod]
        public void ShouldGetAllCategoriesWithUnDisposedConnection()
        {
            var categoryService = ObjectFactory.Container.GetInstance<ICategoryService>();
            var items = categoryService.GetAllCategories();

            Assert.IsTrue(items.Any());
        }


        [TestMethod]
        public void ShouldAdd1CategoryWithNullTitle()
        {
            var rnd = new Random();
            var product4 = new Product { Name = "P400" + rnd.Next(2000), Price = 300 };
            var category1 = new Category
            {
                Name = "Cat1100" + rnd.Next(2000),
                Title = null,
                Products = new List<Product> { product4 }
            };

            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var uow = container.GetInstance<IUnitOfWork>();
                var categoryService = container.GetInstance<ICategoryService>();
                categoryService.AddNewCategory(category1);
                uow.SaveAllChanges();
            }

            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var category = categoryService.FindCategory(category1.Id);

                Assert.IsNotNull(category);
            }
        }

        [TestMethod]
        public void ShouldIssueFullTableScanQuery()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var list = productService.GetAllProductsStartWith("P100");
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void ShouldRunContextInMultipleThreads()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var thread = new Thread(() =>
            {
                var list = productService.GetAllProductsStartWith("P100");
                Assert.IsTrue(list.Any());
            });
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void ShouldIssueIncorrectLazyLoading()
        {
            var categoryService = ObjectFactory.Container.GetInstance<ICategoryService>();
            var list = categoryService.IssueIncorrectLazyLoading();
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void ShouldIssueMultipleEnumeration()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var list = service.GetAllProductsWithPriceGreaterThan(1);
            var count1 = list.Count();
            var any = list.Any();

            Assert.IsTrue(count1 > 0 || any);
        }

        [TestMethod]
        public void ShouldIssueNavigationalCount()
        {
            var service = ObjectFactory.Container.GetInstance<ICategoryService>();
            var count = service.GetFirstCategoryProductsCount();
            Assert.IsTrue(count > 0);
        }
    }
}