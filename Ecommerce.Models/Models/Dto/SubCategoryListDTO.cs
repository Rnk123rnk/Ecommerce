using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class SubCategoryListDTO
    {
        public int SubCategoryId { get; set; }
        public string Category{ get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
