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
    public class SubCategoryRepository : ISubCategoryRepository, IDisposable
    {
        private readonly ApplicationDbContext _db;
        public SubCategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateSubCategory(SubCategory obj)
        {
            if (await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryName == obj.SubCategoryName) != null)
            {
                return false;
            }
            await _db.SubCategorys.AddAsync(obj);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSubCategory(int id)
        {
            var subcategory = await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryId == id);
            if (subcategory != null)
            {
                _db.SubCategorys.Remove(subcategory);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<SubCategory> GetSubCategoryById(int id)
        {
            return await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryId == id);
        }

      
        public async Task<List<SubCategoryListDTO>> GetSubCategoryList()
        {
            var subCategoryListWithCategory = await (from subCategory in _db.SubCategorys
                                                     join category in _db.Categorys
                                                     on subCategory.CategoryId equals category.CategoryId
                                                     select new SubCategoryListDTO
                                                     {
                                                         SubCategoryId = subCategory.SubCategoryId,
                                                         SubCategoryName = subCategory.SubCategoryName,
                                                         IsActive = subCategory.IsActive,
                                                         Category = category.CategoryName
                                                     }).ToListAsync();
            return subCategoryListWithCategory;
        }

        public async Task<List<SubCategory>> GetByIdSubCategoryList(int id)
        {
            return await _db.SubCategorys.Where(u=>u.CategoryId == id).ToListAsync();
        }
        public async Task<int> UpdateSubCategory(SubCategory obj)
        {
            if (await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryName == obj.SubCategoryName && u.SubCategoryId != obj.SubCategoryId) != null)
            {
                return 1;
            }
            var subcategory = await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryId == obj.SubCategoryId);
            if (subcategory == null)
            {
                return 2;
            }
            _db.Entry(subcategory).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;
        }
        public async Task<bool> SubCategoryIdExit(int id)
        {
            if (await _db.SubCategorys.FirstOrDefaultAsync(u => u.SubCategoryId == id) != null)
            {
                return true;
            }
            return false;
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}