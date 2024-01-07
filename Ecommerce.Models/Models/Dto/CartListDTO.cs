using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class CartListDTO
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductPrice { get; set; }
        public int ProductPriceDiscount { get; set; }
    }
}
