﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Models
{
    [Table("ProductReview")]
    public class ProductReview
    {
        [Key]
        public int ProductReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ReviewDescription { get; set; }
        public int Rating { get; set; } 
        public DateTime ReviewDate { get; set; } 
        public bool? IsActive { get; set; } 
    }
}
