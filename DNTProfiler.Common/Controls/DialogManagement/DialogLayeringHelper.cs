using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;

namespace DNTProfiler.Common.Controls.DialogManagement
{
    public class DialogLayeringHelper : IDialogHost
	{
        private readonly List<object> _layerStack = new List<object>();
        private readonly ContentControl _parent;

        public DialogLayeringHelper(ContentControl parent)
		{
			_parent = parent;
		}

		public bool HasDialogLayers { get { return _layerStack.Any(); } }

        public FrameworkElement GetCurrentContent()
        {
            return _parent;
        }

        public void HideDialog(DialogBaseControl dialog)
        {
            if (Equals(_parent.Content, dialog))
            {
                var oldContent = _layerStack.Last();
                _layerStack.Remove(oldContent);
                _parent.Content = oldContent;
            }
            else
                _layerStack.Remove(dialog);
        }

        public void ShowDialog(DialogBaseControl dialog)
		{
			_layerStack.Add(_parent.Content);
			_parent.Content = dialog;
		}
	}
}