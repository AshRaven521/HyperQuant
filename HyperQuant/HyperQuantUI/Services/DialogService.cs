using System.Windows;

namespace HyperQuantUI.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowErrorMessage(string errMessage)
        {
            MessageBox.Show(errMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarningMessage(string warnMessage)
        {
            MessageBox.Show(warnMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
