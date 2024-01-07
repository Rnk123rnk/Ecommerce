using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Models.Dto
{
    public class UsersDTO
    {

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name must be at most 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(50, ErrorMessage = "Email must be at most 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 characters")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Mobile number must contain only numbers")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
    ErrorMessage = "Password must contain at least one letter, one digit, and one special character.")]
        public string Password { get; set; }

    }
}
