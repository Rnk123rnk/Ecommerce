using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IProductReviewRepository
    {
        Task<List<ProductReview>> GetProductReviewList();
        Task<List<ProductReviewListForReviewPage>> GetProductReviewByProductId(int ProductId);
        Task<bool> CreateProductReview(ProductReview obj);
        Task<bool> DeleteProductReview(int id);
        Task<bool> UpdateProductReview(ProductReview obj);
    }
}
