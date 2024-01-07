using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ProductSpecsDTO
    {

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Specification name is required")]
        public string SpecificationName { get; set; }

        [Required(ErrorMessage = "Specification value is required")]
        public string SpecificationValue { get; set; }
    }
}
