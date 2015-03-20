using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class CacheableTests
    {
        [TestMethod]
        public void RunGetCacheableList()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var list = productService.GetAllProductsAsCacheableList();
            Assert.IsNotNull(list);
        }
    }
}