using SKWPFTaskManager.Client.Models.Extensions;
using SKWPFTaskManager.Common.Models;
using System.Windows.Media.Imaging;

namespace SKWPFTaskManager.Client.Models
{
    public class TaskClient
    {
        public TaskModel Model { get; private set; }
        public TaskClient(TaskModel model)
        {
            Model = model;
        }
        public UserModel Creator { get; set; }
        public UserModel Executor { get; set; }
        public BitmapImage Image
        {
            get
            {
                return Model.LoadImage();
            }
        }
        public bool HasFile 
        { 
            get => Model?.File != null;
        }

    }
}
