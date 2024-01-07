using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class OrderDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "TotalAmount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a positive number")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "PaymentMode is required")]
        public string PaymentMode { get; set; }
        public string? RazorpayPaymentId {  get; set; }

        public ICollection<OrderDetailDTO> OrderDetails { get; set; }
        public OrderShippingDTO Shipping { get; set; }
    }
}
