using SKWPFTaskManager.Client.ViewModels;
using SKWPFTaskManager.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace SKWPFTaskManager.Client.Views.AddWindows
{
    /// <summary>
    /// Interaction logic for AddUsersToProjectWindow.xaml
    /// </summary>
    public partial class AddUsersToProjectWindow : Window
    {
        public AddUsersToProjectWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (ProjectsPageViewModel)DataContext;
            foreach (UserModel user in e.RemovedItems)
                viewModel.SelectedUsersForProject.Remove(user);

            foreach (UserModel user in e.AddedItems)
                viewModel.SelectedUsersForProject.Add(user);
        }
    }
}
