using DNTProfiler.Common.Controls.DialogManagement;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.ViewModels;

namespace DNTProfiler
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(new CommonDialogsService(), new DialogManager(this, Dispatcher));
        }
    }
}