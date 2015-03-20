using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;

namespace DNTProfiler.Common.Controls.DialogManagement
{
	partial class DialogBaseControl : INotifyPropertyChanged
	{
        private readonly DialogBase _dialog;
        private HorizontalAlignment _horizontalDialogAlignment = HorizontalAlignment.Center;
        private VerticalAlignment _verticalDialogAlignment = VerticalAlignment.Center;

        public DialogBaseControl(FrameworkElement originalContent, DialogBase dialog)
		{
			Caption = dialog.Caption;

			InitializeComponent();

            var backgroundImage = originalContent.CaptureImage();
            backgroundImage.Stretch = Stretch.Fill;
            BackgroundImageHolder.Content = backgroundImage;

			_dialog = dialog;
			createButtons();
		}

        public event PropertyChangedEventHandler PropertyChanged;

        public string Caption { get; private set; }

		public Visibility CaptionVisibility
		{
			get
			{
				return string.IsNullOrWhiteSpace(Caption)
					? Visibility.Collapsed
					: Visibility.Visible;
			}
		}

        public HorizontalAlignment HorizontalDialogAlignment
        {
            get { return _horizontalDialogAlignment; }
            set
            {
                _horizontalDialogAlignment = value;
                onPropertyChanged("HorizontalDialogAlignment");
            }
        }

        public VerticalAlignment VerticalDialogAlignment
		{
			get { return _verticalDialogAlignment; }
			set
			{
				_verticalDialogAlignment = value;
				onPropertyChanged("VerticalDialogAlignment");
			}
		}

        public void AddCancelButton()
        {
            addButton(_dialog.CancelText, getCallback(_dialog.OnCancelClicked, DialogResultState.Cancel), false, true, "CanCancel");
        }

        public void AddNoButton()
        {
            addButton(_dialog.NoText, getCallback(_dialog.OnNoClicked, DialogResultState.No), false, true, "CanNo");
        }

        public void AddOkButton()
        {
            addButton(_dialog.OkText, getCallback(_dialog.OnOkClicked, DialogResultState.Ok), true, true, "CanOk");
        }

        public void AddYesButton()
        {
            addButton(_dialog.YesText, getCallback(_dialog.OnYesClicked, DialogResultState.Yes), true, false, "CanYes");
        }

        public void SetCustomContent(object content)
		{
			CustomContent.Content = content;
		}

        internal void RemoveButtons()
        {
            ButtonsGrid.Children.Clear();
        }

        private void addButton(
            string buttonText,
            Action callback,
            bool isDefault,
            bool isCancel,
            string bindingPath)
        {
            var btn = new Button
            {
                Content = buttonText,
                IsDefault = isDefault,
                IsCancel = isCancel,
                Margin = new Thickness(5),
                FontFamily = new FontFamily("Tahoma"),
                Width = 30,
                Height = 30
            };

            var enabledBinding = new Binding(bindingPath) { Source = _dialog };
            btn.SetBinding(IsEnabledProperty, enabledBinding);

            btn.Click += (s, e) => callback();

            ButtonsGrid.Columns++;
            ButtonsGrid.Children.Add(btn);
        }

        private void createButtons()
		{
			switch (_dialog.Mode)
			{
				case DialogMode.None:
					break;
				case DialogMode.Ok:
					AddOkButton();
					break;
				case DialogMode.Cancel:
					AddCancelButton();
					break;
				case DialogMode.OkCancel:
					AddOkButton();
					AddCancelButton();
					break;
				case DialogMode.YesNo:
					AddYesButton();
					AddNoButton();
					break;
				case DialogMode.YesNoCancel:
					AddYesButton();
					AddNoButton();
					AddCancelButton();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private Action getCallback(
			Action dialogCallback,
			DialogResultState result)
		{
			_dialog.Result = result;
			Action callback = () =>
			{
				if (dialogCallback != null)
					dialogCallback();
				if (_dialog.CloseBehavior == DialogCloseBehavior.AutoCloseOnButtonClick)
					_dialog.Close();
			};

			return callback;
		}

		private void onPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}