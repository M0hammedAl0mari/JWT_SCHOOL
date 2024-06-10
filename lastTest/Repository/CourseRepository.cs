using lastTest.DataBase;
using lastTest.Models;
using Microsoft.EntityFrameworkCore;

namespace lastTest.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly MyContext _context;

        public CourseRepository(MyContext context)
        {
            _context = context;
        }
        public Student GetUserWithCourses(int userId)
        {
            var x= _context.Students
                .Include(u => u.UserCourses)
                .ThenInclude(uc => uc.Course)
                .FirstOrDefault(u => u.UserId == userId);
            return x;
        }

        public StudentCourse GetUserCourse(int userId, int courseId)
        {
            return _context.StudentCourses
                .Include(uc => uc.Course)
                .Include(uc => uc.student)
                .FirstOrDefault(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public void DeleteUserCourse(StudentCourse userCourse)
        {
            _context.StudentCourses.Remove(userCourse);
            _context.SaveChanges();
        }
    }
}
