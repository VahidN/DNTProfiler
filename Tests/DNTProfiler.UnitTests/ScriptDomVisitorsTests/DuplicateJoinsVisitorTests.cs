using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class DuplicateJoinsVisitorTests
    {
        [TestMethod]
        public void TestSqlQueryHasDuplicateJoinsToSameTable()
        {
            const string sql = @"SELECT * FROM table1
                                    INNER JOIN table2 ON table1.column_name = table2.column_name
                                    left Outer JOIN table2 ON table1.column_name = table2.column_name";

            var visitor = FindEqualityJoinVisitor.Run(sql);
            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestSqlQueryHasNotDuplicateJoinsToSameTable()
        {
            const string sql = @"SELECT * FROM table1
                                    INNER JOIN table2 ON table1.column_name = table2.column_name
                                    left Outer JOIN table3 ON table1.column_name = table3.column_name";

            var visitor = FindEqualityJoinVisitor.Run(sql);
            Assert.IsFalse(visitor.IsSuspected);
        }

        [TestMethod]
        public void TestSqlQueryWithAliasesHasDuplicateJoinsToSameTable()
        {
            // https://entityframework.codeplex.com/workitem/486
            // from: http://stackoverflow.com/questions/12289070/entity-framework-appears-to-be-needlessly-joining-the-same-table-twice
            const string sql = @"SELECT TOP (25)
[Project1].[ReceiptId] AS [ReceiptId],
[Project1].[PublicationId] AS [PublicationId],
[Project1].[DateInserted] AS [DateInserted],
[Project1].[DateReceived] AS [DateReceived],
[Project1].[PublicationId1] AS [PublicationId1],
[Project1].[PayloadId] AS [PayloadId],
[Project1].[TopicId] AS [TopicId],
[Project1].[BrokerType] AS [BrokerType],
[Project1].[DateInserted1] AS [DateInserted1],
[Project1].[DateProcessed] AS [DateProcessed],
[Project1].[DateUpdated] AS [DateUpdated],
[Project1].[PublicationGuid] AS [PublicationGuid],
[Project1].[ReceiptCount] AS [ReceiptCount]
FROM ( SELECT
    [Extent1].[ReceiptId] AS [ReceiptId],
    [Extent1].[PublicationId] AS [PublicationId],
    [Extent1].[DateInserted] AS [DateInserted],
    [Extent1].[DateReceived] AS [DateReceived],
    [Extent3].[PublicationId] AS [PublicationId1],
    [Extent3].[PayloadId] AS [PayloadId],
    [Extent3].[TopicId] AS [TopicId],
    [Extent3].[BrokerType] AS [BrokerType],
    [Extent3].[DateInserted] AS [DateInserted1],
    [Extent3].[DateProcessed] AS [DateProcessed],
    [Extent3].[DateUpdated] AS [DateUpdated],
    [Extent3].[PublicationGuid] AS [PublicationGuid],
    [Extent3].[ReceiptCount] AS [ReceiptCount]
    FROM   [dbo].[Receipt] AS [Extent1]
    INNER JOIN [dbo].[Publication] AS [Extent2] ON [Extent1].[PublicationId] = [Extent2].[PublicationId]
    LEFT OUTER JOIN [dbo].[Publication] AS [Extent3] ON [Extent3].[PublicationId] = [Extent1].[PublicationId]
    WHERE ([Extent2].[ReceiptCount] > 1) AND ([Extent1].[DateInserted] < @p__linq__0) AND ([Extent1].[ReceiptId] > @p__linq__1) AND ([Extent2].[TopicId] = @p__linq__2)
)  AS [Project1]";

            var visitor = FindEqualityJoinVisitor.Run(sql);
            Assert.IsTrue(visitor.IsSuspected);
        }
    }
}