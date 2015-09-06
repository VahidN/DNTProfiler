using System.Collections.Generic;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class UseFirstLevelCacheVisitorTests
    {
        [TestMethod]
        public void TestQueryWithTop1AndKeyComparision()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[MyProductId] = @p__linq__0";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithTop1AndCompositeKeyComparision()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[MyProductId] = @p__linq__0 and
                                    [Extent1].[UserId] = 1";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId",
                    "UserId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }


        [TestMethod]
        public void TestQueryWithTop1AndKeyComparisionWithoutAlias()
        {
            const string sql = @"SELECT TOP (1)
                                    [MyProductId],
                                    [Name],
                                    [Price],
                                    [CategoryId],
                                    [UserId]
                                    FROM [Products]
                                    WHERE [MyProductId] = @p__linq__0";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithTop1AndNotKeyComparision()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[Price] = @p__linq__0";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithoutTop1AndKeyComparision()
        {
            const string sql = @"SELECT
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[MyProductId] = @p__linq__0";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithTop1AndMultipleNonKeyComparisions()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[MyProductId] = @p__linq__0 and [Extent1].[UserId] = 1";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithTop1AndMultipleNonKeyComparisions2()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[Price] = @p__linq__0 and [Extent1].[UserId] = 1";

            var visitor = new UseFirstLevelCacheVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }


        [TestMethod]
        public void TestQueryWithTop1AndMultipleNonKeyComparisions3()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[Price] = @p__linq__0 and [Extent1].[UserId] = 1
                                    and  [Extent1].[MyProductId] = 1";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestQueryWithTop1AndKeyNotEqualComparision()
        {
            const string sql = @"SELECT TOP (1)
                                    [Extent1].[MyProductId] AS [MyProductId],
                                    [Extent1].[Name] AS [Name],
                                    [Extent1].[Price] AS [Price],
                                    [Extent1].[CategoryId] AS [CategoryId],
                                    [Extent1].[UserId] AS [UserId]
                                    FROM [dbo].[Products] AS [Extent1]
                                    WHERE [Extent1].[MyProductId] <> @p__linq__0";

            var visitor = new UseFirstLevelCacheVisitor
            {
                Keys = new HashSet<string>
                {
                    "MyProductId"
                }
            };
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsFalse(visitor.IsSuspected);
        }
    }
}