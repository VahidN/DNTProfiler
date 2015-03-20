using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UnboundedResultSets.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void RunAnalysisVisitor(CommandResult commandResult)
        {
            if (hasNotMoreThanOneReturnedRow(commandResult))
            {
                return;
            }

            var command = PluginContext.ProfilerData.Commands
                .FirstOrDefault(x => x.CommandId == commandResult.CommandId &&
                                     x.ApplicationIdentity.Equals(commandResult.ApplicationIdentity));
            if (command == null)
                return;

            RunAnalysisVisitorOnCommand(new UnboundedResultSetVisitor(), command);
        }

        private static bool hasNotMoreThanOneReturnedRow(CommandResult commandResult)
        {
            return !string.IsNullOrWhiteSpace(commandResult.Exception) ||
                   commandResult.RowsReturned == null ||
                   commandResult.RowsReturned.Value <= 1;
        }
    }
}