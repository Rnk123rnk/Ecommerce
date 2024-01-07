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
    public class ProductSpecsRepository : IProductSpecsRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductSpecsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateProductSpecs(ProductSpecs obj)
        {
            if (await _db.ProductSpecss.FirstOrDefaultAsync(u => u.SpecificationName == obj.SpecificationName && u.ProductId == obj.ProductId) == null)
            {
                await _db.ProductSpecss.AddAsync(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
           
        }

        public async Task<bool> DeleteProductSpecs(int id)
        {
            var ProductSpec = await _db.ProductSpecss.FirstOrDefaultAsync(u => u.ProductSpecsId == id);
            if (ProductSpec != null)
            {
                _db.ProductSpecss.Remove(ProductSpec);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<ProductSpecs> GetProductSpecsById(int id)
        {
            return await _db.ProductSpecss.FirstOrDefaultAsync(u => u.ProductSpecsId == id);
        }

        public async Task<List<ProductSpecs>> GetProductSpecsByProductId(int id)
        {
            return await _db.ProductSpecss.Where(u => u.ProductId == id).ToListAsync();
        }
        public async Task<int> UpdateProductSpecs(ProductSpecs obj)
        {
            if (await _db.ProductSpecss.FirstOrDefaultAsync(u => u.SpecificationName == obj.SpecificationName && u.ProductSpecsId != obj.ProductSpecsId && u.ProductId == obj.ProductId) != null)
            {
                return 1;
            }
            var ProductSpec = await _db.ProductSpecss.FirstOrDefaultAsync(u => obj.ProductSpecsId == u.ProductSpecsId);
            if (ProductSpec == null)
            {
                return 2;
            }
            _db.Entry(ProductSpec).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;
        }
    }
}
