using lastTest.DataBase;
using lastTest.Models;
using Microsoft.EntityFrameworkCore;

namespace lastTest.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MyContext _context;

        public StudentRepository(MyContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentCourse> GetStudentCourses(int studentId)
        {
            return _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.UserId == studentId)
                .ToList();
        }

        public void RegisterCourses(int studentId, int[] courseIds)
        {
            foreach (var courseId in courseIds)
            {
                var studentCourse = new StudentCourse
                {
                    UserId = studentId,
                    CourseId = courseId
                };
                _context.StudentCourses.Add(studentCourse);
            }
            Save();
        }

        public void DeleteCourse(int studentId, int courseId)
        {
            var studentCourse = _context.StudentCourses
                .FirstOrDefault(sc => sc.UserId == studentId && sc.CourseId == courseId);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
            }
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
