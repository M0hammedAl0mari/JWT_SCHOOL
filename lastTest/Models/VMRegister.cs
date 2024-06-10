using System.ComponentModel.DataAnnotations;

namespace lastTest.Models
{
    public class VMRegister
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        public int Age { get; set; } // For students
        public string Gender { get; set; } // For teachers
    }
}
