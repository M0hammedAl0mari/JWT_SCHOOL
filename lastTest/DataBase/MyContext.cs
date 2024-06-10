using Microsoft.EntityFrameworkCore;
using lastTest.Models;

namespace lastTest.DataBase
{
    public class MyContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.UserId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.student)
                .WithMany(s => s.UserCourses)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.TeacherCourses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Roles data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Student" },
                new Role { Id = 2, Name = "Teacher" }
            );

            // Seed Students data
            modelBuilder.Entity<Student>().HasData(
                new Student { UserId = 1, Name = "Student1", Email = "student1@example.com", Password = "password", Age = 20, RoleId = 1 },
                new Student { UserId = 2, Name = "Student2", Email = "student2@example.com", Password = "password", Age = 22, RoleId = 1 }
            );

            // Seed Teachers data
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { UserId = 3, Name = "Teacher1", Email = "teacher1@example.com", Password = "password", gender = "Male", RoleId = 2 },
                new Teacher { UserId = 4, Name = "Teacher2", Email = "teacher2@example.com", Password = "password", gender = "Female", RoleId = 2 }
            );

            // Seed Courses data
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, Name = "Course1", Description = "Description1", TeacherId = 3 },
                new Course { CourseId = 2, Name = "Course2", Description = "Description2", TeacherId = 4 }
            );
        }
    }
}
