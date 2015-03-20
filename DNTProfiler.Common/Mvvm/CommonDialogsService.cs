using Microsoft.Win32;

namespace DNTProfiler.Common.Mvvm
{
    public interface ICommonDialogsService
    {
        string ShowSaveFileDialogGetPath(string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*");
    }

    public class CommonDialogsService : ICommonDialogsService
    {
        public string ShowSaveFileDialogGetPath(string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter
            };
            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return string.Empty;

            return dialog.FileName;
        }
    }
}