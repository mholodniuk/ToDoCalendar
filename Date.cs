using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCalendar
{
    public class Date
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
