using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("ProductWishlist")] 
    public class ProductWishlist
    {
        [Key]
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
