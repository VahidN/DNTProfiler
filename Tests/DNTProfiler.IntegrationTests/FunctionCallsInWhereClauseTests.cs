using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class FunctionCallsInWhereClauseTests
    {
        [TestMethod]
        public void ShouldEmitToUpperFunction()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var product = productService.FindProduct("P100");
            Assert.IsNotNull(product);
        }
    }
}