using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ProductListForClientDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int ProductMRPPrice { get; set; }
        public int? ProductDiscountedPrice { get; set; }
        public string? ProductImage1 { get; set; }
        public string? CategoryName { get; set; }
        public int AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int MinProductPrice { get; set; }
        public int MaxProductPrice { get; set; }


    }
}
