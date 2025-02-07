using Microsoft.Win32;
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

        public string OpenFile()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.Title = "Выберите файл конфигурации";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }
    }
}
