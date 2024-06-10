using System.ComponentModel.DataAnnotations.Schema;

namespace lastTest.Models
{
 
    public class StudentCourse
    {
        public int UserId { get; set; }
        public Student student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
