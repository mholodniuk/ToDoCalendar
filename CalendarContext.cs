using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace ToDoCalendar
{
    /// <summary>
    /// Database context that enables application to communicate with a (in-memory) database
    /// Database schema contains two sets: Dates and Activities
    /// </summary>
    public class CalendarContext : DbContext
    {
        public CalendarContext() : base("CalendarDatabase")
        {

        }
        public virtual DbSet<Date> Dates { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
    }

    /// <summary>
    /// Database Initializer class that seeds database on application startup
    /// Database is created only if it has not been created before (in order to save data after application closes)
    /// </summary>
    public class CalendarDbInitializer : CreateDatabaseIfNotExists<CalendarContext>
    {
        /// <summary>
        /// Main seeding method that runs every time application needs to seed database
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(CalendarContext context)
        {
            var dates = new List<Date>
                {
                    new Date() 
                    { 
                        ID=1,
                        Day=DateTime.Today.AddDays(2),
                        Activities = new Collection<Activity>
                        {
                            new Activity()
                            {
                                ID = 3,
                                Name = "Bieg",
                                StartTime = "13:00",
                                Done = true,
                                DateID = 1
                            }
                        }
                    },
                    new Date() 
                    { 
                        ID=2, 
                        Day=DateTime.Now,
                        Activities = new Collection<Activity>
                        {
                            new Activity()
                            {
                                ID = 1,
                                Name = "Bieg",
                                StartTime ="10:00",
                                Done = false,
                                DateID = 2
                            },
                            new Activity()
                            {
                                ID = 2,
                                Name = "Pływanie",
                                StartTime = "19:30",
                                Done = false,
                                DateID = 2
                            },
                            new Activity()
                            {
                                ID = 4,
                                Name = "Skakanie",
                                StartTime = "17:30",
                                Done = true,
                                DateID = 2
                            },
                            new Activity()
                            {
                                ID = 4,
                                Name = "Jeżdzenie",
                                StartTime = "7:15",
                                Done = false,
                                DateID = 2
                            }
                        }
                    }
                };
            dates.ForEach(c => context.Dates.Add(c));
            context.SaveChanges();
        }
    }
}
