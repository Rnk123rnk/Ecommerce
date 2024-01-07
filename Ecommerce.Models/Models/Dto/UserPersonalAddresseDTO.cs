
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Models.Dto
{
    public class UserPersonalAddresseDTO
    {

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number. It must be 10 digits.")]

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string MobileNumber { get; set; }

        [StringLength(10, ErrorMessage = "Pin code must be at most 10 characters")]
        public int? PinCode { get; set; }

    }
}
