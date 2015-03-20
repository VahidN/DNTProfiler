namespace DNTProfiler.Common.Controls.DialogManagement.Contracts
{
    public interface  IDialogManager
    {
        IDialog CreateCustomContentDialog(object content, DialogMode dialogMode);
        IDialog CreateCustomContentDialog(object content, string caption, DialogMode dialogMode);
    }
}