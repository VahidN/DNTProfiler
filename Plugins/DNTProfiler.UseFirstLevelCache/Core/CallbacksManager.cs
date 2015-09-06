using System.Collections.Generic;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UseFirstLevelCache.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void RunAnalysisVisitor(CommandResult commandResult)
        {
            var command = PluginContext.ProfilerData.Commands
               .FirstOrDefault(x => x.CommandId == commandResult.CommandId &&
                                    x.ApplicationIdentity.Equals(commandResult.ApplicationIdentity));
            if (command == null)
                return;

            RunAnalysisVisitorOnCommand(() =>
            {
                var visitor = new UseFirstLevelCacheVisitor
                {
                    Keys = getPrimaryKeys(commandResult)
                };
                var isUsingKeysInQuery = RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(command.Sql, command.SqlHash, visitor);
                if (!isUsingKeysInQuery)
                {
                    return false;
                }

                return isUsingFirstOrDefault(command);
            }, command);
        }

        private static HashSet<string> getPrimaryKeys(CommandResult commandResult)
        {
            var keys = new HashSet<string>();
            foreach (var column in commandResult.Columns.Where(column => column.IsKey))
            {
                keys.Add(column.ColumnName);
            }
            return keys;
        }

        private static bool isUsingFirstOrDefault(BaseInfo command)
        {
            var isUsingFindMethod = command.StackTrace.CallingMethodInfoList.Any(
                methodInfo => methodInfo.AssemblyName.StartsWith("EntityFramework") &&
                              (methodInfo.CallingMethod.Equals("FindInStore") || methodInfo.CallingMethod.Equals("FindInStoreAsync")));
            if (isUsingFindMethod)
            {
                return false;
            }

            var isUsingFirstOrDefault = command.StackTrace.CallingMethodInfoList.Any(
                methodInfo => methodInfo.AssemblyName.StartsWith("EntityFramework") &&
                              (methodInfo.CallingMethod.Equals("ExecuteSingle") || methodInfo.CallingMethod.Equals("ExecuteSingleAsync")));
            return isUsingFirstOrDefault;
        }
    }
}