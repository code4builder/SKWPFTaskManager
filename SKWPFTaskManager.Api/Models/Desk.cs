using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKWPFTaskManager.Api.Models
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        //TODO: Json format for columns
        public string Columns { get; set; }
        public int? AdminId { get; set; }
        public User Admin { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
