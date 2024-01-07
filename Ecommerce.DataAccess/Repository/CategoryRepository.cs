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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CategoryIdExit(int id)
        {
            if (await _db.Categorys.FirstOrDefaultAsync(u => u.CategoryId == id) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CategordNameExit(string Name)
        {
            if (await _db.Categorys.FirstOrDefaultAsync(u => u.CategoryName == Name) != null)
            {
                return true;
            }
            return false;
        }
        public async Task CreateCategory(Category obj)
        {
                await _db.Categorys.AddAsync(obj);
                await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var Category = await _db.Categorys.FirstOrDefaultAsync(u => u.CategoryId == id);
            if (Category != null)
            {
                _db.Categorys.Remove(Category);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _db.Categorys.FirstOrDefaultAsync(u => u.CategoryId == id);
        }

        public Task<List<Category>> GetCategoryList()
        {
            return _db.Categorys.ToListAsync();
        }

        public async Task<int> UpdateCategory(Category obj)
        {
            if (await _db.Categorys.FirstOrDefaultAsync(u => obj.CategoryName == u.CategoryName && u.CategoryId != obj.CategoryId) != null)
            {
                return 1;
            }
            var category = await _db.Categorys.FirstOrDefaultAsync(u => obj.CategoryId == u.CategoryId);
            if (category == null)
            {
                return 2;
            }
            _db.Entry(category).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;

        }

        public async Task<List<Category>> MenuFill()
        {
            var result = await _db.Categorys.Include(c => c.SubCategories).ThenInclude(sc => sc.ThirdCategories).ToListAsync();
            return result;

        }
    }
}
