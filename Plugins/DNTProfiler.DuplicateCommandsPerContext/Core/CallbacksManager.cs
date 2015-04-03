using System;
using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerContext.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<Context> _localContexts = new ObservableCollection<Context>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageCommand(Command item)
        {
            if (!hasThisContextDuplicateQueriesWithSameHash(item))
                return;

            if (shouldIgnoreAddingThisContext(item))
                return;

            var context = GetContext(item);
            if (context == null)
                return;

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item.ApplicationIdentity);

            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.Contexts.Add(context);
            }

            _localContexts.Add(context);
        }

        public void Reset()
        {
            _localContexts.Clear();
        }

        public void ShowSelectedApplicationIdentityLocalContexts()
        {
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.Contexts.Clear();
            GuiModelData.Contexts.Clear();
            GuiModelData.Contexts.Clear();
            GuiModelData.Contexts =
                new ObservableCollection<Context>(
                    _localContexts.Where(context => context.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }

        public void ShowSelectedContextRelatedDuplicateCommands()
        {
            if (GuiModelData.SelectedContext == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();

            var relatedCommands = PluginContext.ProfilerData.Commands
                .Where(
                    command =>
                        command.ObjectContextId == GuiModelData.SelectedContext.ObjectContextId &&
                        command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .ToList();

            foreach (var item in relatedCommands.Where(item => relatedCommands.Any(
                            command => command.CommandId != item.CommandId &&
                           (command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase) ||
                            command.NormalizedSqlHash.Equals(item.NormalizedSqlHash, StringComparison.OrdinalIgnoreCase))))
                            .OrderBy(command => command.SqlHash)
                            .ThenBy(command => command.NormalizedSqlHash))
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        private bool hasThisContextDuplicateQueriesWithSameHash(Command item)
        {
            return PluginContext.ProfilerData.Commands.Any(
                command => command.ObjectContextId == item.ObjectContextId &&
                    command.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                     command.CommandId != item.CommandId &&
                     (command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase) ||
                     command.NormalizedSqlHash.Equals(item.NormalizedSqlHash, StringComparison.OrdinalIgnoreCase)));
        }

        private bool shouldIgnoreAddingThisContext(BaseInfo item)
        {
            return _localContexts.Any(
                context => context.ObjectContextId == item.ObjectContextId &&
                           context.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }
    }
}