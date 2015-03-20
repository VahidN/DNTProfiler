using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;

namespace DNTProfiler.Common.Controls.DialogManagement
{
	public class DialogBase : IDialog, INotifyPropertyChanged
	{
        private readonly IDialogHost _dialogHost;
        private readonly Dispatcher _dispatcher;
        private bool _canCancel;
        private bool _canNo;
        private bool _canOk;
        private bool _canYes;
        private HorizontalAlignment? _horizontalDialogAlignment;
        private VerticalAlignment? _verticalDialogAlignment;

        public DialogBase(
			IDialogHost dialogHost,
			DialogMode dialogMode,
			Dispatcher dispatcher)
		{
			_dialogHost = dialogHost;
			_dispatcher = dispatcher;
			Mode = dialogMode;
			CloseBehavior = DialogCloseBehavior.AutoCloseOnButtonClick;

            OkText = "\u221A";
            CancelText = "\u2212";
            YesText = "\u221A";
            NoText = "\u2212";

			switch (dialogMode)
			{
				case DialogMode.None:
					break;
				case DialogMode.Ok:
					CanOk = true;
					break;
				case DialogMode.Cancel:
					CanCancel = true;
					break;
				case DialogMode.OkCancel:
					CanOk = true;
					CanCancel = true;
					break;
				case DialogMode.YesNo:
					CanYes = true;
					CanNo = true;
					break;
				case DialogMode.YesNoCancel:
					CanYes = true;
					CanNo = true;
					CanCancel = true;
					break;
				default:
					throw new ArgumentOutOfRangeException("dialogMode");
			}

		}

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                _canCancel = value;
                onPropertyChanged("CanCancel");
            }
        }

        public string CancelText { get; set; }

        public bool CanNo
        {
            get { return _canNo; }
            set
            {
                _canNo = value;
                onPropertyChanged("CanNo");
            }
        }

        public bool CanOk
        {
            get { return _canOk; }
            set
            {
                _canOk = value;
                onPropertyChanged("CanOk");
            }
        }

        public bool CanYes
        {
            get { return _canYes; }
            set
            {
                _canYes = value;
                onPropertyChanged("CanYes");
            }
        }

        public string Caption { get; set; }

        public DialogCloseBehavior CloseBehavior { get; set; }

        public object Content { set; get; }

        public HorizontalAlignment HorizontalDialogAlignment
        {
            set
            {
                if (DialogBaseControl == null)
                    _horizontalDialogAlignment = value;
                else
                    DialogBaseControl.HorizontalDialogAlignment = value;
            }
        }

        public DialogMode Mode { get; private set; }

        public string NoText { get; set; }

        public string OkText { get; set; }

        public Action OnCancelClicked { get; set; }
        public Action OnNoClicked { get; set; }
        public Action OnOkClicked { get; set; }
        public Action OnYesClicked { get; set; }

        public DialogResultState Result { get; set; }

        public VerticalAlignment VerticalDialogAlignment
        {
            set
            {
                if (DialogBaseControl == null)
                    _verticalDialogAlignment = value;
                else
                    DialogBaseControl.VerticalDialogAlignment = value;
            }
        }
        public string YesText { get; set; }

        protected DialogBaseControl DialogBaseControl { get; private set; }

        public void Close()
        {
            if (DialogBaseControl == null)
                return;

            OnOkClicked = null;
            OnCancelClicked = null;
            OnYesClicked = null;
            OnNoClicked = null;

            InvokeUiCall(
                () =>
                {
                    _dialogHost.HideDialog(DialogBaseControl);
                    DialogBaseControl.SetCustomContent(null);
                });
        }

        public void Show()
        {
            if (DialogBaseControl != null)
                throw new Exception("The dialog can only be shown once.");

            InvokeUiCall(() =>
                {
                    DialogBaseControl = new DialogBaseControl(_dialogHost.GetCurrentContent(), this);
                    DialogBaseControl.SetCustomContent(Content);
                    if (_verticalDialogAlignment.HasValue)
                        DialogBaseControl.VerticalDialogAlignment = _verticalDialogAlignment.Value;
                    if (_horizontalDialogAlignment.HasValue)
                        DialogBaseControl.HorizontalDialogAlignment = _horizontalDialogAlignment.Value;
                    _dialogHost.ShowDialog(DialogBaseControl);
                });
        }

        protected void InvokeUiCall(Action del)
        {
            _dispatcher.Invoke(del, DispatcherPriority.DataBind);
        }

		private void onPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}