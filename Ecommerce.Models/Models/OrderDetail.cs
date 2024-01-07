using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderDetailId { get; set; } 
        public int OrderId { get; set; } 
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
