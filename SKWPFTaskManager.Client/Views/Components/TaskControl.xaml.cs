using SKWPFTaskManager.Client.Models;
using System;
using System.Windows.Controls;

namespace SKWPFTaskManager.Client.Views.Components
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        public TaskControl(TaskClient task)
        {
            InitializeComponent();
            DataContext = task;
        }
    }
}
