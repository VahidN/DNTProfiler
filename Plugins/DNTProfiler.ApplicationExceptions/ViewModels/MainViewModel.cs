using System;
using System.Windows;
using System.Windows.Threading;
using DNTProfiler.ApplicationExceptions.Core;
using DNTProfiler.ApplicationExceptions.Models;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationExceptions.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            setActions();
            setGuiModel();
            setEvenets();
        }

        public AppExceptionsGui AppExceptionsGuiData { set; get; }

        private void addException(Exception ex)
        {
            AppExceptionsGuiData.AppExceptionsList.Add(new AppException
            {
                Message = ex.Message,
                Details = ExceptionLogger.GetExceptionCallStack(ex)
            });

            DispatcherHelper.DispatchAction(() =>
            {
                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            });
        }

        private void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            new ExceptionLogger().LogExceptionToFile(e.Exception, AppMessenger.LogFile);
            e.Handled = true;

            addException(e.Exception);
        }

        private void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);

            addException(ex);

            if (e.IsTerminating)
            {
                MessageBox.Show(new ExceptionLogger().GetExceptionMessageStack(ex), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
                AppExceptionsGuiData.AppExceptionsList.Clear();
            };

            PluginContext.GetResults = () =>
            {
                return AppExceptionsGuiData.AppExceptionsList.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
            Application.Current.DispatcherUnhandledException += appDispatcherUnhandledException;

            AppMessenger.Messenger.Register<Exception>("ShowException", exception => addException(exception));
        }

        private void setGuiModel()
        {
            AppExceptionsGuiData = new AppExceptionsGui();
            BindingListener.Instance.Start(error =>
            {
                AppExceptionsGuiData.AppExceptionsList.Add(new AppException
                {
                    Message = "BindingExpression path error",
                    Details = error
                });
                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                new ExceptionLogger().LogExceptionToFile(new NotImplementedException(error), AppMessenger.LogFile);
            });
        }
    }
}