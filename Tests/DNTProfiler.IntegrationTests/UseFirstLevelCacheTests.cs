using DNTProfiler.ServiceLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class UseFirstLevelCacheTests
    {

        [TestMethod]
        public void IssueFirstOrDefaultAndFind()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            service.IssueFirstOrDefaultAndFind(id: 1);
        }

        [TestMethod]
        public void IssueFindAndFirstOrDefault()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            service.IssueFindAndFirstOrDefault(id: 1);
        }

        [TestMethod]
        public void TestFindKeyUsingFirstOrDefault()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindKeyUsingFirstOrDefault(id: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindKeyUsingFirstOrDefault1()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindKeyUsingFirstOrDefault(id: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindKeyUsingSingleOrDefault()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindKeyUsingSingleOrDefault(id: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindKeyUsingFindMethod()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindKeyUsingFindMethod(id: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindNonKeyUsingFirstOrDefault1()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindNonKeyUsingFirstOrDefault(minPrice: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindNonKeyUsingFirstOrDefault2()
        {
            var service = ObjectFactory.Container.GetInstance<IProductService>();
            var product1 = service.FindNonKeyUsingFirstOrDefault(minPrice: 1);
            Assert.IsNotNull(product1);
        }

        [TestMethod]
        public void TestFindNonKeyUsingFirstOrDefault3()
        {
            var productService = ObjectFactory.Container.GetInstance<IProductService>();
            var product = productService.FindProduct("P100");
            Assert.IsNotNull(product);
        }
    }
}