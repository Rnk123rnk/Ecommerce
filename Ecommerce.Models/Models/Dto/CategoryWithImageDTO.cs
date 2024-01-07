using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Models.Dto
{
    public class CategoryWithImageDTO
    {
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? CategoryImage { get; set; }
    }
}
