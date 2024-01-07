using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class CompanyDTO
    {

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "IsActive field is required. Enter 'true' or 'false'.")]
        public bool IsActive { get; set; }
    }
}
