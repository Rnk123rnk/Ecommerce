using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models.Dto
{
    public class ProductDTO
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ThirdCategoryId { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int ProductMRPPrice { get; set; }
        public int ProductDiscountedPrice { get; set; }
        public IFormFile? ProductImage1 { get; set; }
        public IFormFile? ProductImage2 { get; set; }
        public IFormFile? ProductImage3 { get; set; }
        public IFormFile? ProductImage4 { get; set; }
        public bool IsActive { get; set; }

    }
}