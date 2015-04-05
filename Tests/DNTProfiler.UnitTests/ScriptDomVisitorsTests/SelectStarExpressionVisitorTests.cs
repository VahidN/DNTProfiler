using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class SelectStarExpressionVisitorTests
    {
        [TestMethod]
        public void TestHasSelectStarExpression()
        {
            const string sql = @"SELECT * from tbl1 where f1 = 12";

            var visitor = new SelectStarExpressionVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }
    }
}