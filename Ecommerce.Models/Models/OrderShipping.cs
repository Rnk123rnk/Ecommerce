using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("OrderShipping")]
    public class OrderShipping
    {
        [Key]
        public int OrderShippingId { get; set; }
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; } 
        public string State { get; set; }
        public string MobileNumber { get; set; }
        public string PinCode { get; set; } 
    }
}
