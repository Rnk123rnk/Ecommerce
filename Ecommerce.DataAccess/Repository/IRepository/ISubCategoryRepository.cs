using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface ISubCategoryRepository
    {
        Task<List<SubCategoryListDTO>> GetSubCategoryList();
        Task<SubCategory> GetSubCategoryById(int id);
        Task<bool> CreateSubCategory(SubCategory obj);
        Task<bool> DeleteSubCategory(int id);
        Task<int> UpdateSubCategory(SubCategory obj);
        Task<bool> SubCategoryIdExit(int id);
        Task<List<SubCategory>> GetByIdSubCategoryList(int id);
    }
}