using lastTest.Models;

namespace lastTest.Repository
{
    public interface ICourseRepository
    {
        Student GetUserWithCourses(int userId);
        StudentCourse GetUserCourse(int userId, int courseId);
        void DeleteUserCourse(StudentCourse userCourse);
        
    }
}
