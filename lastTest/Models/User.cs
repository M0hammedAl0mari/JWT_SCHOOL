namespace lastTest.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // Foreign key for Role
        public Role Role { get; set; } // Navigation property to Role
    }
}
