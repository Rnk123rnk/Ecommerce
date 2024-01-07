using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryImage{ get; set; } 
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }

    }
}
