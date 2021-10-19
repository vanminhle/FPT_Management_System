using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace FPT_Management_System.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("FPTManagementSystem", throwIfV1Schema: false)
        {
        }

        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<TraineesToCourse> TraineesToCourses { get; set; }
        public DbSet<TrainersToCourse> TrainersToCourses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}