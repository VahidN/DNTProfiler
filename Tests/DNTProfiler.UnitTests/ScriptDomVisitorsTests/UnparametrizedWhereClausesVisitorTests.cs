using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class UnparametrizedWhereClausesVisitorTests
    {
        [TestMethod]
        public void CheckComparisonWithStringLiteral()
        {
            const string sql = @"SELECT
                                [GroupBy1].[A1] AS [C1]
                                FROM ( SELECT
                                    COUNT(1) AS [A1]
                                    FROM  [Roles] AS [Extent1]
                                    INNER JOIN  (SELECT [Extent2].[Role_Id] AS [Role_Id], [Extent2].[User_Id] AS [User_Id], [Extent3].[Id] AS [Id], [Extent3].[FriendlyName] AS [FriendlyName], [Extent3].[UserName] AS [UserName], [Extent3].[Password] AS [Password], [Extent3].[EMail] AS [EMail], [Extent3].[IsActive] AS [IsActive], [Extent3].[ReceiveDailyEmails] AS [ReceiveDailyEmails], [Extent3].[EmailIsValidated] AS [EmailIsValidated], [Extent3].[RegistrationCode] AS [RegistrationCode], [Extent3].[NumberOfPosts] AS [NumberOfPosts], [Extent3].[NumberOfComments] AS [NumberOfComments], [Extent3].[NumberOfLinks] AS [NumberOfLinks], [Extent3].[NumberOfProjects] AS [NumberOfProjects], [Extent3].[NumberOfDrafts] AS [NumberOfDrafts], [Extent3].[NumberOfProjectsFeedbacks] AS [NumberOfProjectsFeedbacks], [Extent3].[NumberOfProjectsComments] AS [NumberOfProjectsComments], [Extent3].[NumberOfLinksComments] AS [NumberOfLinksComments], [Extent3].[NumberOfSurveys] AS [NumberOfSurveys], [Extent3].[NumberOfVoteComments] AS [NumberOfVoteComments], [Extent3].[NumberOfAdvertisements] AS [NumberOfAdvertisements], [Extent3].[NumberOfAdvertisementComments] AS [NumberOfAdvertisementComments], [Extent3].[NumberOfCourses] AS [NumberOfCourses], [Extent3].[NumberOfLearningPaths] AS [NumberOfLearningPaths], [Extent3].[LastVisitDateTime] AS [LastVisitDateTime], [Extent3].[IsRestricted] AS [IsRestricted], [Extent3].[HomePageUrl] AS [HomePageUrl], [Extent3].[Photo] AS [Photo], [Extent3].[Description] AS [Description], [Extent3].[DateOfBirth] AS [DateOfBirth], [Extent3].[Location] AS [Location], [Extent3].[IsJobsSeeker] AS [IsJobsSeeker], [Extent3].[IsEmailPublic] AS [IsEmailPublic], [Extent3].[FacebookName] AS [FacebookName], [Extent3].[TwitterName] AS [TwitterName], [Extent3].[LinkedInProfileId] AS [LinkedInProfileId], [Extent3].[GooglePlusProfileId] AS [GooglePlusProfileId], [Extent3].[StackOverflowId] AS [StackOverflowId], [Extent3].[GithubId] AS [GithubId], [Extent3].[NugetId] AS [NugetId], [Extent3].[CodePlexId] AS [CodePlexId], [Extent3].[CodeProjectId] AS [CodeProjectId], [Extent3].[SourceforgeId] AS [SourceforgeId], [Extent3].[Rating_TotalRating] AS [Rating_TotalRating], [Extent3].[Rating_TotalRaters] AS [Rating_TotalRaters], [Extent3].[Rating_AverageRating] AS [Rating_AverageRating], [Extent3].[CreatedOn] AS [CreatedOn], [Extent3].[CreatedOnPersian] AS [CreatedOnPersian], [Extent3].[CreatedBy] AS [CreatedBy], [Extent3].[CreatedByIp] AS [CreatedByIp], [Extent3].[CreatedByBrowserName] AS [CreatedByBrowserName], [Extent3].[ModifiedOn] AS [ModifiedOn], [Extent3].[ModifiedOnPersian] AS [ModifiedOnPersian], [Extent3].[ModifiedBy] AS [ModifiedBy], [Extent3].[ModifiedByIp] AS [ModifiedByIp], [Extent3].[ModifiedByBrowserName] AS [ModifiedByBrowserName]
                                        FROM  [RoleUsers] AS [Extent2]
                                        INNER JOIN [Users] AS [Extent3] ON [Extent3].[Id] = [Extent2].[User_Id] ) AS [Join1] ON [Extent1].[Id] = [Join1].[Role_Id]
                                    WHERE (N'Admin' = [Extent1].[Name]) AND ([Join1].[IsActive] = @p__linq__0)
                                )  AS [GroupBy1]";


            var visitor = new UnparametrizedWhereClausesVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithIntLiteral()
        {
            const string sql = @"SELECT TOP (20)
                                        [Extent1].[Id] AS [Id], [Extent1].[FirstName] AS [FirstName]
                                FROM
                                (SELECT [Extent1].[Id] AS [Id], [Extent1].[FirstName] AS [FirstName],
                                    row_number() OVER (ORDER BY [Extent1].[FirstName] ASC) AS [row_number]
                                    FROM [dbo].[Customers] AS [Extent1])  AS [Extent1]
                                WHERE [Extent1].[row_number] > 10 ORDER BY [Extent1].[FirstName] ASC";

            var visitor = new UnparametrizedWhereClausesVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }

        [TestMethod]
        public void CheckComparisonWithIntLiteralWithParenthesis()
        {
            const string sql = @"SELECT TOP (20)
                                        [Extent1].[Id] AS [Id], [Extent1].[FirstName] AS [FirstName]
                                FROM
                                (SELECT [Extent1].[Id] AS [Id], [Extent1].[FirstName] AS [FirstName],
                                    row_number() OVER (ORDER BY [Extent1].[FirstName] ASC) AS [row_number]
                                    FROM [dbo].[Customers] AS [Extent1])  AS [Extent1]
                                WHERE (([Extent1].[row_number]) > (10)) ORDER BY [Extent1].[FirstName] ASC";

            var visitor = new UnparametrizedWhereClausesVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);

            Assert.IsTrue(visitor.IsSuspected);
        }
    }
}