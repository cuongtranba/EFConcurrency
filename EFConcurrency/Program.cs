using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConcurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.EntityFramework.
    EntityFrameworkProfiler.Initialize();
            Student student1WithUser1 = null;
            Student student1WithUser2 = null;

            //User 1 gets student
            using (var context = new StudentDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                student1WithUser1 = context.Students.Where(s => s.StudentId == 1).Single();
            }
            //User 2 also get the same student
            using (var context = new StudentDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                student1WithUser2 = context.Students.Where(s => s.StudentId == 1).Single();
            }
            //User 1 updates Student name
            student1WithUser1.StudentName = "Edited from user1";

            //User 2 updates Student name
            student1WithUser2.StudentName = "Edited from user2";

            using (var context = new StudentDbContext())
            {
                try
                {
                    context.Entry(student1WithUser1).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Optimistic Concurrency exception occured");
                }
            }

            //User 2 saves changes after User 1. 
            //User 2 will get concurrency exection 
            //because CreateOrModifiedDate is different in the database 
            using (var context = new StudentDbContext())
            {
                try
                {
                    context.Entry(student1WithUser2).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Optimistic Concurrency exception occured");
                }
            }
        }
    }
}
