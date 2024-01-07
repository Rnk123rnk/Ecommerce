using Ecommerce.Models.Models.Dto;
using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoryList();
        Task<Category> GetCategoryById(int id);
        Task CreateCategory(Category obj);
        Task<bool> DeleteCategory(int id);
        Task<int> UpdateCategory(Category obj);
        Task<bool> CategoryIdExit(int id);
        Task<List<Category>> MenuFill();
        Task<bool> CategordNameExit(string Name);
    }
}
