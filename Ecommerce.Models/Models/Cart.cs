using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("Cart")]
    public class Cart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; } 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
