using Prism.Mvvm;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SKWPFTaskManager.Client.Services
{
    public class CommonViewService
    {
        private string _imageDialogFilterPattern = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        public Window CurrentOpenedWindow { get; private set; }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowActionResult(System.Net.HttpStatusCode code, string message)
        {
            if (code == System.Net.HttpStatusCode.OK)
            {
                ShowMessage(code.ToString() + $"\n{message}");
            }
            else
            {
                ShowMessage(code.ToString() + "\nError! Something wrong");
            }
        }

        public void OpenWindow(Window window, BindableBase viewModel)
        {
            CurrentOpenedWindow = window;
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        public string GetFileFromDialog(string filter)
        {
            string filePath = String.Empty;

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = filter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                filePath = dialog.FileName;

            return filePath;
        }

        public CommonModel SetPhotoForObject(CommonModel model)
        {
            string photoPath = GetFileFromDialog(_imageDialogFilterPattern);
            if (string.IsNullOrEmpty(photoPath) == false)
            {
                var photoBytes = File.ReadAllBytes(photoPath);
                model.Photo = photoBytes;
            }
            return model;
        }
    }
}
