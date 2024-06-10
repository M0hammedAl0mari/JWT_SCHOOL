using lastTest.Models;

namespace lastTest.Repository
{
    public interface ITeacherRepository
    {
        IEnumerable<Course> GetCoursesByTeacher(int teacherId);
        Course GetCourseById(int courseId);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int courseId);
        void Save();
        int GetTeacherIdByCourseId(Course course);

    }
}