using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ToDoCalendar
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("StudentDatabase") {}
        public DbSet<Student> Students { get; set; }
    }

    public class StudentDbInitializer : DropCreateDatabaseAlways<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            var courses = new List<Student>
                {
                    new Student() { ID=1, Name="Adam"},
                    new Student() { ID=2, Name="Ewa"},
                };
            courses.ForEach(c => context.Students.Add(c));
            context.SaveChanges();
        }
    }
}
