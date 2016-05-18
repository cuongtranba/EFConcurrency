using System.Data.Entity;

namespace EFConcurrency
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext():base("name=StudentDb")
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}
