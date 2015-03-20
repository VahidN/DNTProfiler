using System;
using System.Windows.Controls;
using System.Windows.Threading;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;

namespace DNTProfiler.Common.Controls.DialogManagement
{
    public class DialogManager : IDialogManager
    {
        private readonly IDialogHost _dialogHost;
        private readonly Dispatcher _dispatcher;

        public DialogManager(ContentControl parent, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _dialogHost = new DialogLayeringHelper(parent);
        }

        public IDialog CreateCustomContentDialog(object content, DialogMode dialogMode)
        {
            IDialog dialog = null;
            invokeOnUiThread(() =>
            {
                dialog = new DialogBase(_dialogHost, dialogMode, _dispatcher) { Content = content };
            });
            return dialog;
        }

        public IDialog CreateCustomContentDialog(object content, string caption, DialogMode dialogMode)
        {
            IDialog dialog = null;
            invokeOnUiThread(() =>
            {
                dialog = new DialogBase(_dialogHost, dialogMode, _dispatcher)
                {
                    Caption = caption,
                    Content = content
                };
            });
            return dialog;
        }

        private void invokeOnUiThread(Action del)
        {
            _dispatcher.Invoke(del);
        }
    }
}