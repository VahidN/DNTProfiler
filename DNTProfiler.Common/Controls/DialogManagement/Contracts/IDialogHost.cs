using System.Windows;

namespace DNTProfiler.Common.Controls.DialogManagement.Contracts
{
    public interface IDialogHost
	{
		void ShowDialog(DialogBaseControl dialog);
		void HideDialog(DialogBaseControl dialog);
		FrameworkElement GetCurrentContent();
	}
}