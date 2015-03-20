using System.Linq;
using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class CompareWithNullTests
    {
        [TestMethod]
        public void ShouldReturnNoCategoryTitleWhenNullIsVariable()
        {
            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var list = categoryService.GetNullCategoryTitles();
                Assert.IsTrue(!list.Any());
            }
        }

        [TestMethod]
        public void ShouldReturnCategoryTitleWhenNullIsNotVariable()
        {
            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var list = categoryService.GetNullCategoryTitlesSolution1();
                Assert.IsTrue(list.Any());
            }
        }

        [TestMethod]
        public void ShouldReturnNoCategoryNameWhenNullIsVariable()
        {
            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var list = categoryService.GetNullCategoryNames();
                Assert.IsTrue(!list.Any());
            }
        }

        [TestMethod]
        public void ShouldReturnEmptyCategoryNameWhenNullIsNotVariable()
        {
            using (var container = ObjectFactory.Container.GetNestedContainer())
            {
                var categoryService = container.GetInstance<ICategoryService>();
                var list = categoryService.GetNullCategoryNamesSolution1();
                Assert.IsTrue(!list.Any());
            }
        }
    }
}