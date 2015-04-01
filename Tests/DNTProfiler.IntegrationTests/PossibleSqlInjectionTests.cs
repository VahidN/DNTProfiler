using DNTProfiler.TestEFContext.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.IntegrationTests
{
    [TestClass]
    public class PossibleSqlInjectionTests
    {
        [TestMethod]
        public void ShouldEmitPossibleSqlInjection1()
        {
            var unitOfWork = ObjectFactory.Container.GetInstance<IUnitOfWork>();
            var name = "P100";
            var rows = unitOfWork.GetRows<int>(
                "select id from Products where Name like '" + name + "%'");
            Assert.IsNotNull(rows);
        }
    }
}
