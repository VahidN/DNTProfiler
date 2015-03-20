using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class ArithmeticOverflowVisitorTests
    {
        [TestMethod]
        public void CheckSelectSumIsSuspectedToArithmeticOverflow()
        {
            const string sql = @"select sum(f1) from tbl1;";
            var visitor = new ArithmeticOverflowVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckSelectSumWithParenthesisIsSuspectedToArithmeticOverflow()
        {
            const string sql = @"select (sum((f1))) from tbl1;";
            var visitor = new ArithmeticOverflowVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckEfSelectSumIsSuspectedToArithmeticOverflow()
        {
            const string sql = @"SELECT
                                     [GroupBy1].[A1] AS [C1]
                                     FROM ( SELECT
                                                SUM([Extent1].[Amount]) AS [A1]
                                                FROM [dbo].[Transactions] AS [Extent1]
                                                )  AS [GroupBy1]";
            var visitor = new ArithmeticOverflowVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckSelectSumCastAsBigIntIsNotSuspectedToArithmeticOverflow()
        {
            const string sql = @"SELECT
                                  [GroupBy1].[A1] AS [C1]
                                     FROM ( SELECT
                                                SUM( CAST( [Extent1].[Amount] AS bigint)) AS [A1]
                                                FROM [dbo].[Transactions] AS [Extent1]
                                           )  AS [GroupBy1]";
            var visitor = new ArithmeticOverflowVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckSelectCountIsNotSuspectedToArithmeticOverflow()
        {
            const string sql = @"SELECT [GroupBy1].[A1] AS [C1]
                                 FROM   (SELECT COUNT(1) AS [A1]
                                        FROM   [dbo].[__MigrationHistory] AS [Extent1]
                                        WHERE  [Extent1].[ContextKey] = @p__linq__0) AS [GroupBy1];";
            var visitor = new ArithmeticOverflowVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }
    }
}