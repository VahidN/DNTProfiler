using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByException.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void AddExceptionalCommands(CommandResult commandResult)
        {
            if (string.IsNullOrWhiteSpace(commandResult.Exception))
                return;

            var command = PluginContext.ProfilerData.Commands
                                        .FirstOrDefault(cmd => cmd.CommandId == commandResult.CommandId &&
                                                        cmd.ApplicationIdentity.Equals(commandResult.ApplicationIdentity));
            if (command == null)
                return;

            if (commandResult.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedCommands.Add(command);
            }

            GuiModelData.LocalCommands.Add(command);

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(commandResult);
        }
    }
}