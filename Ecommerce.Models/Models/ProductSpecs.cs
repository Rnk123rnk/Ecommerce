using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("ProductSpecs")]
    public class ProductSpecs
    {
        [Key]
        public int ProductSpecsId { get; set; }
        public int ProductId { get; set; }
        public string SpecificationName { get; set; }
        public string SpecificationValue { get; set; } 
    }
}
