using System;
using System.Collections.Generic;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class CommandStatisticsRun
    {
        private static readonly Dictionary<string, CommandStatistics> _commandStatistics = new Dictionary<string, CommandStatistics>();

        public static void SetCommandStatistics(this Command command)
        {
            if (command == null)
                return;

            try
            {
                runAnalysis(command);
            }
            catch (Exception ex)
            {
                new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);
                AppMessenger.Messenger.NotifyColleagues("ShowException", ex);
            }
        }

        private static void runAnalysis(Command command)
        {
            CommandStatistics result;
            if (_commandStatistics.TryGetValue(command.SqlHash, out result))
            {
                command.CommandStatistics = result;
            }
            else
            {
                var counterVisitor = new CounterVisitor();
                RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(command.Sql, command.SqlHash, counterVisitor);

                command.CommandStatistics = counterVisitor.CommandStatistics;
                _commandStatistics.Add(command.SqlHash, counterVisitor.CommandStatistics);
            }
        }
    }
}