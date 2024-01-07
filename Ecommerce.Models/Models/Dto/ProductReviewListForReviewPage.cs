using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ProductReviewListForReviewPage
    {
        public int ProductReviewId { get; set; }
        public string UserName { get; set; }
        public string ReviewDescription { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
