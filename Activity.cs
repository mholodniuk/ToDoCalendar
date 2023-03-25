using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCalendar
{
    public class Activity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public bool Done { get; set; }

        [ForeignKey("Date")]
        public int DateID { get; set; }
        public virtual Date Date { get; set; }
    }
}
