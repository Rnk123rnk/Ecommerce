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
    public class ThirdCategoryRepository : IThirdCategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public ThirdCategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async
            Task<bool> CreateThirdCategory(ThirdCategory obj)
        {
            if (await _db.ThirdCategorys.FirstOrDefaultAsync(u => u.ThirdCategoryName == obj.ThirdCategoryName) != null)
            {
                return false;
            }
            await _db.ThirdCategorys.AddAsync(obj);
            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteThirdCategory(int id)
        {
            var Thirdcategory = await _db.ThirdCategorys.FirstOrDefaultAsync(u => u.ThirdCategoryId == id);
            if (Thirdcategory != null)
            {
                _db.ThirdCategorys.Remove(Thirdcategory);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ThirdCategory>> GetByIdThirdCategoryList(int id)
        {
            return await _db.ThirdCategorys.Where(u => u.SubCategoryId == id).ToListAsync();
        }

        public async Task<ThirdCategory> GeThirdCategoryById(int id)
        {
            return await _db.ThirdCategorys.FirstOrDefaultAsync(u => u.ThirdCategoryId == id);
        }

        public async Task<List<ThirdCategoryListDTO>> GetThirdCategoryList()
        {
            var ThirdCategoryListWithCategory = await (from thirdcategory in _db.ThirdCategorys
                                                       join category in _db.Categorys
                                                       on thirdcategory.CategoryId equals category.CategoryId
                                                       join subcategory in _db.SubCategorys
                                                       on thirdcategory.SubCategoryId equals subcategory.SubCategoryId
                                                       select new ThirdCategoryListDTO
                                                       {
                                                           ThirdCategoryId = thirdcategory.ThirdCategoryId,
                                                           ThirdCategoryName = thirdcategory.ThirdCategoryName,
                                                           IsActive = thirdcategory.IsActive,
                                                           Category = category.CategoryName,
                                                           SubCategory = subcategory.SubCategoryName,
                                                       }).ToListAsync();
            return ThirdCategoryListWithCategory;
        }

        public async Task<int> UpdateThirdCategory(ThirdCategory obj)
        {
            if (await _db.ThirdCategorys.FirstOrDefaultAsync(u => u.ThirdCategoryName == obj.ThirdCategoryName && u.ThirdCategoryId != obj.ThirdCategoryId) != null)
            {
                return 1;
            }
            var thirdcategory = await _db.ThirdCategorys.FirstOrDefaultAsync(u => obj.ThirdCategoryId == u.ThirdCategoryId);
            if (thirdcategory == null)
            {
                return 2;
            }
            _db.Entry(thirdcategory).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;
        }
    }
}
