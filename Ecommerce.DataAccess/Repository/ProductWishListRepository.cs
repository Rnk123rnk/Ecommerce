using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class ProductWishListRepository : IProductWishListRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductWishListRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateOrRemoveWishList(ProductWishlist obj)
        {
            var existingWishList = await _db.ProductWishlists.FirstOrDefaultAsync(u => u.ProductId == obj.ProductId && u.UserId == obj.UserId);
            if (existingWishList != null)
            {
                _db.ProductWishlists.Remove(existingWishList);
                await _db.SaveChangesAsync();
                return false;
            }
            await _db.ProductWishlists.AddAsync(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> WishListExit(int UserId, int ProductId)
        {
            var existingWishList = await _db.ProductWishlists.FirstOrDefaultAsync(u => u.ProductId == ProductId && u.UserId == UserId);
            if (existingWishList != null)
            {
                return true;
            }
            return false;
        
        }
    }
}
