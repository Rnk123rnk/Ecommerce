using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsShipped { get; set; }
        public string PaymentMode { get; set; }
        public string? RazorpayPaymentId {  get; set; }
        public bool IsPaymentReceived { get; set; }
        public bool IsCancel { get; set; }
        public string ProductStatus { get; set; } 
    }
}
