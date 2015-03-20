using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class CounterVisitorTests
    {
        [TestMethod]
        public void CheckSelectsCounts()
        {
            const string sql = @"select * from tbl1; select * from tbl2;";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(2, counterVisitor.CommandStatistics.SelectsCount);
        }

        [TestMethod]
        public void CheckInsertsCount()
        {
            const string sql = @"
              INSERT [dbo].[Categories]([Name], [Title])
              VALUES (@0, @1);
              SELECT [Id] FROM [dbo].[Categories]
              WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity();
              INSERT [dbo].[Categories]([Name], [Title])
              VALUES (@0, @1);
              SELECT [Id] FROM [dbo].[Categories]
              WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity();";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(2, counterVisitor.CommandStatistics.InsertsCount);
        }

        [TestMethod]
        public void CheckUpdatesCount()
        {
            const string sql = @"
                    UPDATE [Users]  SET [LastVisitDateTime] = @0 WHERE ([Id] = @1);
                    SELECT [Id] FROM [dbo].[Categories];
                    UPDATE [Users]  SET [LastVisitDateTime] = @0 WHERE ([Id] = @1);";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(2, counterVisitor.CommandStatistics.UpdatesCount);
        }

        [TestMethod]
        public void CheckDeletesCount()
        {
            const string sql = @"
                DELETE [dbo].[Products] WHERE ([Id] = @0);
                SELECT [Id] FROM [dbo].[Categories];
                DELETE [dbo].[Products] WHERE ([Id] = @0);";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(2, counterVisitor.CommandStatistics.DeletesCount);
        }

        [TestMethod]
        public void CheckLikesCount()
        {
            const string sql = @"
                    SELECT * FROM Person.Contact
                    WHERE LastName LIKE '%mith';
                    SELECT [Id] FROM [dbo].[Categories];
                    SELECT * FROM Person.Contact
                    WHERE LastName LIKE '%' + @blah + '%';";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(2, counterVisitor.CommandStatistics.LikesCount);
        }

        [TestMethod]
        public void CheckJoinsCount()
        {
            const string sql = @"
                    SELECT *
                    FROM table1 INNER JOIN table2
                    ON table1.column_name = table2.column_name;

                    SELECT
                        [Extent1].[Id] AS [Id],
                        [Extent1].[Name] AS [Name]
                    FROM
                        [dbo].[Items] AS [Extent1]
                        INNER JOIN [dbo].[CountryItem] AS [Extent2]
                                                   ON [Extent1].[Id] = [Extent2].[Items_Id]
                        INNER JOIN (
                            SELECT
                                [UnionAll1].[C1] AS [C1]
                            FROM (
                                SELECT 1 AS [C1] FROM (SELECT 1 AS X) AS [SingleRowTable1]
                                UNION ALL
                                SELECT 2 AS [C1] FROM (SELECT 1 AS X) AS [SingleRowTable2]
                            ) AS [UnionAll1]
                            UNION ALL
                            SELECT 3 AS [C1] FROM (SELECT 1 AS X) AS [SingleRowTable3]
                        ) AS [UnionAll2] ON [Extent2].[Countries_Id] = [UnionAll2].[C1];

                    Select column_name(s)
                    from table1
                    cross join table2;";
            var counterVisitor = new CounterVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), counterVisitor);

            Assert.AreEqual(4, counterVisitor.CommandStatistics.JoinsCount);
        }
    }
}