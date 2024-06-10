using lastTest.DataBase;
using lastTest.Models;
using lastTest.Repository;
using System.Collections.Generic;
using System.Linq;

namespace lastTest.Data
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly MyContext _context;

        public TeacherRepository(MyContext context)
        {
            _context = context;
        }
        public IEnumerable<Course> GetCoursesByTeacher(int teacherId)
        {
            return _context.Courses.Where(c => c.TeacherId == teacherId).ToList();
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
        }

        public void DeleteCourse(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public int GetTeacherIdByCourseId(Course course)
        {
            var courseId = course.CourseId;
            var c = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            return c?.TeacherId ?? 0;
        }

    }
}
