using SKWPFTaskManager.Client.ViewModels;
using SKWPFTaskManager.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace SKWPFTaskManager.Client.Views.AddWindows
{
    /// <summary>
    /// Interaction logic for UsersFromExcelWindow.xaml
    /// </summary>
    public partial class UsersFromExcelWindow : Window
    {
        public UsersFromExcelWindow()
        {
            InitializeComponent();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = DataContext as UsersPageViewModel;


            foreach (var item in e.RemovedItems)
            {
                if (item.GetType() == typeof(UserModel))
                {
                    var user = (UserModel)item;
                    if (context.SelectedUsersFromExcel.Contains(user))
                        context.SelectedUsersFromExcel.Remove(user);
                }
            }
            foreach (var item in e.AddedItems)
            {
                if (item.GetType() == typeof(UserModel))
                {
                    var user = (UserModel)item;
                    context.SelectedUsersFromExcel.Add(user);
                }
            }

        }
    }
}
