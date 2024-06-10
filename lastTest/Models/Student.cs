using System.ComponentModel.DataAnnotations.Schema;

namespace lastTest.Models
{
    
    public class Student : User
    {
        public int Age { get; set; }
        public ICollection<StudentCourse> UserCourses { get; set; } = new List<StudentCourse>();
    }
}
