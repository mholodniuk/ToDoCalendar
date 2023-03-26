using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ToDoCalendar
{
    public class CalendarContext : DbContext
    {
        public CalendarContext() : base("CalendarDatabase")
        {

        }
        public  DbSet<Date> Dates { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }

    public class CalendarDbInitializer : DropCreateDatabaseAlways<CalendarContext>
    {
        protected override void Seed(CalendarContext context)
        {
            var dates = new List<Date>
                {
                    new Date() { ID=1, Day=DateTime.Now},
                    new Date() { ID=2, Day=DateTime.Now},
                };
            dates.ForEach(c => context.Dates.Add(c));
            context.SaveChanges();
        }
    }
}
