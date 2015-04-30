using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FloatingPointFields.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void RunAnalysisVisitor(CommandResult commandResult)
        {
            if (commandResult.Columns == null || !commandResult.Columns.Any())
            {
                return;
            }

            if (!commandResult.Columns.Any(column => column.DataType == "System.Single" ||
                                                     column.DataType == "System.Double")) return;
            var command = PluginContext.ProfilerData.Commands
                .FirstOrDefault(cmd => cmd.CommandId == commandResult.CommandId &&
                                     cmd.ApplicationIdentity.Equals(commandResult.ApplicationIdentity));
            if (command == null)
                return;

            if (Equals(GuiModelData.SelectedApplicationIdentity, command.ApplicationIdentity))
            {
                GuiModelData.RelatedCommands.Add(command);
            }
            GuiModelData.LocalCommands.Add(command);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(command);
        }
    }
}