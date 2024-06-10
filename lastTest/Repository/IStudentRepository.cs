using lastTest.Models;

namespace lastTest.Repository
{
    public interface IStudentRepository
    {
        IEnumerable<StudentCourse> GetStudentCourses(int studentId);
        void RegisterCourses(int studentId, int[] courseIds);
        void DeleteCourse(int studentId, int courseId);
        void Save();
    }
}
