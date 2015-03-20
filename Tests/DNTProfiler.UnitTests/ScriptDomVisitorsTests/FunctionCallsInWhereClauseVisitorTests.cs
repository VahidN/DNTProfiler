using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class FunctionCallsInWhereClauseVisitorTests
    {
        [TestMethod]
        public void CheckHasUpperFunction()
        {
            const string sql = @"select sum(f1) from tbl1 where upper(f2)='TEST'";

            var visitor = new FunctionCallsInWhereClauseVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckFunctionInSelectStatementIsNotSuspected()
        {
            const string sql = @"select sum(f1) from tbl1";

            var visitor = new FunctionCallsInWhereClauseVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckFunctionAsValueIsNotSuspected()
        {
            const string sql = @"SELECT [Id] FROM [dbo].[Products] WHERE
                                 @@ROWCOUNT > 0 AND [Id] = scope_identity()";

            var visitor = new FunctionCallsInWhereClauseVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckHasUpperFunctionWithParenthesis()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[Id] AS [Id],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE ((UPPER([Extent1].[Name])) = (UPPER('Test'))) OR
                                          ((UPPER([Extent1].[Name]) IS NULL) AND
                                          (UPPER('Test') IS NULL))";

            var visitor = new FunctionCallsInWhereClauseVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }
    }
}