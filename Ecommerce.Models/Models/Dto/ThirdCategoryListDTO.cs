using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ThirdCategoryListDTO
    {
        public int ThirdCategoryId { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ThirdCategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
