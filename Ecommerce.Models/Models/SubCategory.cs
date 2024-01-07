using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("SubCategory")]
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual Category Category { get; set; }

        // Navigation property to ThirdCategory
        public virtual ICollection<ThirdCategory> ThirdCategories { get; set; }
    }
}
