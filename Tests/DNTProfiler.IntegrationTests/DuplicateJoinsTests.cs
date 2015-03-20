using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class DuplicateJoinsTests
    {
        [TestMethod]
        public void ShouldEmitQueryWithDuplicateJoins()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var products = productService.GetProductsWithMultipleJoins();
            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void TestIfEmitsQueryWithDuplicateJoinsWithLet()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var products = productService.GetProductsWithMultipleJoinsWithLet();
            Assert.IsNotNull(products);
        }
    }
}