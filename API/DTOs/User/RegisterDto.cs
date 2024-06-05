using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 4)]
        public string Password { get; set; }
    }
}