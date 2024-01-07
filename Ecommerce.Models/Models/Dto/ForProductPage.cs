using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ForProductPage
    {
        public int ProductPageMaxPrice { get; set; }
        public int ProductPageMinPrice { get; set; }
        public int TotalProductCount { get; set; }
    }
}
