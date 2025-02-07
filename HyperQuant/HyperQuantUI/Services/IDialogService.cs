namespace HyperQuantUI.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        void ShowErrorMessage(string errMessage);
        void ShowWarningMessage(string warnMessage);
        string OpenFile();
    }
}
