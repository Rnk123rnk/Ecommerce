using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class OrderShippingDTO
    {
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "MobileNumber is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number. It must be 10 digits.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "PinCode is required")]
        public string PinCode { get; set; }
    }
}
