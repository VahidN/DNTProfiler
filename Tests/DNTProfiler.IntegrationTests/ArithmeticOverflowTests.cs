using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class ArithmeticOverflowTests
    {
        [TestMethod]
        public void ShouldEmitSumPrice()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var sum = productService.GetTotalPriceSumInt();
            Assert.IsTrue(sum > 0);
        }

        [TestMethod]
        public void ShouldEmitSumCastAsBigIntPrice()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var sum = productService.GetTotalPriceSumLong();
            Assert.IsTrue(sum > 0);
        }
    }
}