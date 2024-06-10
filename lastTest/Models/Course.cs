using System.ComponentModel.DataAnnotations.Schema;

namespace lastTest.Models
{
  
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StudentCourse> UserCourses { get; set; } = new List<StudentCourse>();

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
