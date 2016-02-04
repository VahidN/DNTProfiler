using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class ParametrizedSkipAndTakeTests
    {
        [TestMethod]
        public void CheckQueryHasNotParametrizedSkipAndTake()
        {
            const string sql = @"SELECT
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[MyProducts] AS [Extent1]
                                    ORDER BY [Extent1].[MyProductId] ASC
                                    OFFSET 8 ROWS FETCH NEXT 8 ROWS ONLY";

            var visitor = new UnParametrizedSkipAndTakeVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckQueryHasNotParametrizedSkipAndTakeWithParenthesis()
        {
            const string sql = @"SELECT
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[MyProducts] AS [Extent1]
                                    ORDER BY [Extent1].[MyProductId] ASC
                                    OFFSET (8) ROWS FETCH NEXT (8) ROWS ONLY";

            var visitor = new UnParametrizedSkipAndTakeVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckQueryHasParametrizedSkipAndTake()
        {
            const string sql = @"SELECT
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[MyProducts] AS [Extent1]
                                    ORDER BY [Extent1].[MyProductId] ASC
                                    OFFSET @p__linq__0 ROWS FETCH NEXT @p__linq__1 ROWS ONLY";

            var visitor = new UnParametrizedSkipAndTakeVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }
    }
}