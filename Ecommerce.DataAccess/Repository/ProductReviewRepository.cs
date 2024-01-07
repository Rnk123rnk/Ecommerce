using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductReviewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateProductReview(ProductReview obj)
        {
            if( await _db.ProductReviews.FirstOrDefaultAsync(u=> u.ProductId == obj.ProductId && u.UserId ==obj.UserId) != null)
            {
                return false;
            }
            await _db.ProductReviews.AddAsync(obj);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductReview(int id)
        {
            var ProductReview = await _db.ProductReviews.FirstOrDefaultAsync(u=> u.ProductReviewId == id);
            if (ProductReview != null)
            {
                _db.ProductReviews.Remove(ProductReview);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ProductReviewListForReviewPage>> GetProductReviewByProductId(int Productid)
        {

            var ProductReview = await (from productreview in _db.ProductReviews
                                       join user in _db.Userss
                                       on productreview.UserId equals user.UserId
                                       where productreview.ProductId ==Productid && productreview.IsActive == true
                                       select new ProductReviewListForReviewPage
                                       {
                                           UserName = user.FullName,
                                           ProductReviewId = productreview.ProductId,
                                           Rating = productreview.Rating,
                                           ReviewDescription = productreview.ReviewDescription,
                                           ReviewDate = DateTime.Now
                                       }).ToListAsync();
            return ProductReview;

    
        }

        public async Task<List<ProductReview>> GetProductReviewList()
        {
            return await _db.ProductReviews.ToListAsync();
        }
        public async Task<bool> UpdateProductReview(ProductReview obj)
        {
            var ProductReview = await _db.ProductReviews.FirstOrDefaultAsync(u => obj.ProductReviewId == u.ProductReviewId);
            if (ProductReview == null)
            {
                return false;
            }
            _db.Entry(ProductReview).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return true;

        }
    }
}
