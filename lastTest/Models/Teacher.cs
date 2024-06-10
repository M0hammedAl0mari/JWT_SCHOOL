namespace lastTest.Models
{
    public class Teacher : User
    {

        public string gender { get; set; }
        public ICollection<Course> TeacherCourses { get; set; } = new List<Course>();
    }
}
