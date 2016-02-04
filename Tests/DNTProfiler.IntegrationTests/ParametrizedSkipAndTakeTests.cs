using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class ParametrizedSkipAndTakeTests
    {
        [TestMethod]
        public void ShouldEmitQueryWithParametrizedSkipAndTake()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var products = productService.GetProductsPagedListWithParametrizedSkipAndTake(pageNumber: 1);
            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void ShouldEmitQueryWithoutParametrizedSkipAndTake()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var products = productService.GetProductsPagedListWithoutParametrizedSkipAndTake(pageNumber: 1);
            Assert.IsNotNull(products);
        }
    }
}
