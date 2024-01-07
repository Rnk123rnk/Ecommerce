using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class SingleProductForClientDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int ProductMRPPrice { get; set; }
        public int? ProductDiscountedPrice { get; set; }
        public string? ProductImage1 { get; set; }
        public string? ProductImage2 { get; set; }
        public string? ProductImage3 { get; set; }
        public string? ProductImage4 { get; set; }
        public int Quantity { get; set; }
        public int AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int OneStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int FiveStarCount { get; set; }
        public string? CategoryName { get; set; }

    }
}
