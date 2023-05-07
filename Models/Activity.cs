using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCalendar
{
    /// <summary>
    /// Class represents Activity entity
    /// It stores basic properties needed for user interactions and database persistance
    /// 
    ///   This entity has many-to-one relationship with Date entity (on Date can contain many (or none) Activities, 
    ///   but one Activity is assigned to only one Date)
    /// </summary>
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
