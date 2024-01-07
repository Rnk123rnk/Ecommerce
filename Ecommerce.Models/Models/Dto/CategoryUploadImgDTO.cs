using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class CategoryUploadImgDTO
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string? OldCategoryImage { get; set; }
        public IFormFile? CategoryImage { get; set; }
    }
}
