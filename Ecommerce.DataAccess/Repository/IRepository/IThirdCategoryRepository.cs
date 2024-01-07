using Ecommerce.Models.Models.Dto;
using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IThirdCategoryRepository
    {
        Task<List<ThirdCategoryListDTO>> GetThirdCategoryList();
        Task<ThirdCategory> GeThirdCategoryById(int id);
        Task<bool> CreateThirdCategory(ThirdCategory obj);
        Task<bool> DeleteThirdCategory(int id);
        Task<int> UpdateThirdCategory(ThirdCategory obj);
        Task<List<ThirdCategory>> GetByIdThirdCategoryList(int id);
    }
}
