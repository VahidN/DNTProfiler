using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByTransactions.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void TransactionBegan(CommandTransaction item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                !GuiModelData.RelatedTransactions.Contains(item))
            {
                GuiModelData.RelatedTransactions.Add(item);
            }

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item);
        }

        public void TransactionCommitted(CommandTransaction item)
        {
            var transaction = GetTransaction(item);
            if (transaction != null)
            {
                transaction.ClosedAt = item.AtDateTime;
            }
        }

        public void TransactionRolledBack(CommandTransaction item)
        {
            var transaction = GetTransaction(item);
            if (transaction != null)
            {
                transaction.RolledBackAt = item.AtDateTime;
            }
        }

        public void TransactionDisposed(CommandTransaction item)
        {
            var transaction = GetTransaction(item);
            if (transaction != null)
            {
                transaction.DisposedAt = item.AtDateTime;
            }
        }
    }
}