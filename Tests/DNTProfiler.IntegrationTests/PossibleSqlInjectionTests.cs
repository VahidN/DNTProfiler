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

        [TestMethod]
        public void ShouldEmitPossibleSqlInjection2()
        {
            var unitOfWork = ObjectFactory.Container.GetInstance<IUnitOfWork>();
            var name = "P100";
            var result = unitOfWork.ExecuteSqlCommand(
                "update Products set Price = 100 where Name like '" + name + "%'");
            Assert.IsTrue(result > 0);
        }
    }
}