using System.Collections.ObjectModel;
using System.IO;
using DNTProfiler.Infrastructure.Models;

namespace DNTProfiler.Dumper.Models
{
    public class MainGuiModel : GuiModelBase
    {
        private DumperSettings _dumperSettings;
        private ObservableCollection<FileInfo> _files;
        private FileInfo _selectedFile;

        public MainGuiModel()
        {
            Files = new ObservableCollection<FileInfo>();
            DumperSettings = new DumperSettings();
        }


        public DumperSettings DumperSettings
        {
            get { return _dumperSettings; }
            set
            {
                _dumperSettings = value;
                NotifyPropertyChanged(() => DumperSettings);
            }
        }

        public ObservableCollection<FileInfo> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                NotifyPropertyChanged(() => Files);
            }
        }

        public FileInfo SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                NotifyPropertyChanged(() => SelectedFile);
            }
        }
    }
}