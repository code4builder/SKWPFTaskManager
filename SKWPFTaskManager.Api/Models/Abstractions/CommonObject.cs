using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKWPFTaskManager.Api.Models
{
    public class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[]? Photo { get; set; }
        public CommonObject()
        {
            CreationDate = DateTime.Now;
        }
    }
}
