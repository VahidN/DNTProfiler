using System;
using System.Windows;

namespace DNTProfiler.Common.Controls.DialogManagement.Contracts
{
    public interface IDialog
    {
        DialogMode Mode { get; }
        DialogResultState Result { get; }
        DialogCloseBehavior CloseBehavior { get; set; }

        Action OnOkClicked { get; set; }
        Action OnCancelClicked { get; set; }
        Action OnYesClicked { get; set; }
        Action OnNoClicked { get; set; }

        bool CanOk { get; set; }
        bool CanCancel { get; set; }
        bool CanYes { get; set; }
        bool CanNo { get; set; }

        string OkText { get; set; }
        string CancelText { get; set; }
        string YesText { get; set; }
        string NoText { get; set; }

        string Caption { get; set; }

        VerticalAlignment VerticalDialogAlignment { set; }
        HorizontalAlignment HorizontalDialogAlignment { set; }

        void Show();
        void Close();
    }
}