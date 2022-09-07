namespace SKWPFTaskManager.Common.Models
{
    public class TaskModel : CommonModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public byte[]? File { get; set; }
        public int DeskId { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public int? ExecutorId { get; set; }

        public TaskModel() { }

        public TaskModel(string name, string description, DateTime start, DateTime finish, string column)
        {
            Name = name;
            Description = description;
            StartDate = start; 
            FinishDate = finish;
            Column = column;
        }
    }


}
