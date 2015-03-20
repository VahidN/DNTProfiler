using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class UnboundedResultSetVisitorTests
    {
        [TestMethod]
        public void CheckQueryHasOffsetFetch()
        {
            const string sql = @"SELECT BusinessEntityID, FirstName, LastName
                                FROM Testoffset
                                ORDER BY BusinessEntityID
                                OFFSET 3 ROWS
                                FETCH First 3 ROWS only";

            var visitor = new UnboundedResultSetVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckQueryWithoutWhereClauses()
        {
            const string sql = @"SELECT BusinessEntityID, FirstName, LastName
                                FROM Testoffset";

            var visitor = new UnboundedResultSetVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckQueryHasWhereClauses()
        {
            const string sql = @"SELECT BusinessEntityID, FirstName, LastName
                                FROM Testoffset
                                Where BusinessEntityID>3";

            var visitor = new UnboundedResultSetVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckQueryHasTopKeyword()
        {
            const string sql = @"SELECT Top 3 BusinessEntityID, FirstName, LastName
                                FROM Testoffset";

            var visitor = new UnboundedResultSetVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }
    }
}