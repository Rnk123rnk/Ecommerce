using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ThirdCategoryId { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public int ProductMRPPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public int ProductDiscountedPrice { get; set; }
        public string? ProductImage1 { get; set; }
        public string? ProductImage2 { get; set; }
        public string ?ProductImage3 { get; set; }
        public string? ProductImage4 { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get;set; }
    }
}
