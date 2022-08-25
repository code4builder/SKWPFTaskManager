namespace SKWPFTaskManager.Common.Models
{
    public class DeskModel : CommonModel
    {
        public bool IsPrivate { get; set; }
        public string[] Columns { get; set; }
        public int? AdminId { get; set; }
        public int? ProjectId { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
