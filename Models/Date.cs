using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCalendar
{
    /// <summary>
    /// Class represents Date entity
    /// It stores basic properties needed for user interactions and database persistance
    /// 
    ///   This entity has one-to=many relationship with Activity entity (on Date can contain many (or none) Activities, 
    ///   but one Activity is assigned to only one Date)
    /// </summary>
    public class Date
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
