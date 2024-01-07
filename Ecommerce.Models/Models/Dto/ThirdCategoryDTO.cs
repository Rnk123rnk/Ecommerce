using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ThirdCategoryDTO
    {

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "SubCategory ID is required")]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "ThirdCategory Name is required")]
        [StringLength(50, ErrorMessage = "ThirdCategory Name must be at most 50 characters")]
        public string ThirdCategoryName { get; set; }

        [Required(ErrorMessage = "IsActive field is required. Enter 'true' or 'false'.")]
        public bool IsActive { get; set; }

    }
}
