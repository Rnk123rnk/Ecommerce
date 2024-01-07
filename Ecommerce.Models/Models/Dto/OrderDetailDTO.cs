using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class OrderDetailDTO
    {
        [Required(ErrorMessage = "ProductId is required")]
        public int productId { get; set; }

        [Required(ErrorMessage = "ProductName is required")]
        public string productName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int quantity { get; set; }

        [Required(ErrorMessage = "productPrice is required")]
        [Range(0, double.MaxValue, ErrorMessage = "productPrice must be a positive number")]
        public decimal productPrice { get; set; }

        [Required(ErrorMessage = "productPricDiscount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "productPricDiscount must be a positive number")]
        public decimal productPriceDiscount { get; set; }
    }
}
