using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("ThirdCategory")]
    public class ThirdCategory
    {
        [Key]
        public int ThirdCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string ThirdCategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
        // Navigation property to SubCategory
        public virtual SubCategory SubCategory { get; set; }
    }
}
