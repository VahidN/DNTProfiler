using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class NullComparisonVisitorTests
    {
        [TestMethod]
        public void CheckComparisonWithNullLiteral()
        {
            var sql = @"select * from tbl1 where
                        data='test' and name=null";

            var visitor = new NullComparisonVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithNullLiteralWithParenthesis()
        {
            var sql = @"select * from tbl1 where
                        ((data='test') and (name=null))";

            var visitor = new NullComparisonVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithNullLiteralWithExtraParenthesis()
        {
            var sql = @"select * from tbl1 where
                        (((data)=('test')) and ((name)=(null)))";

            var visitor = new NullComparisonVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithNullTSqlVariable()
        {
            var sql = @"decalre @x = null
                        select * from tbl1 where
                        data='test' and name=@x";

            var visitor = new NullComparisonVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithNullVariable()
        {
            var sql = @"select * from tbl1 where
                        data='test' and name=@x";

            var visitor = new NullComparisonVisitor();
            visitor.NullVariableNames.Add("@x");
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonNullWithNullVariable()
        {
            var sql = @"SELECT [Extent1].[Id] AS [Id],
                               [Extent1].[Name] AS [Name],
                               [Extent1].[Title] AS [Title]
                        FROM   [dbo].[Categories] AS [Extent1]
                        WHERE  ([Extent1].[Title] = @p__linq__0) -- this shouldn't be here
                               OR (([Extent1].[Title] IS NULL)
                                   AND (@p__linq__0 IS NULL));";

            var visitor = new NullComparisonVisitor();
            visitor.NullVariableNames.Add("@p__linq__0");
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }
    }
}