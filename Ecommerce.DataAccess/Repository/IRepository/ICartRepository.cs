using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<List<CartListDTO>> GetCartByIdList(int id,string randomId);
        Task<bool> CreateCart(Cart obj);
        Task<bool> DeleteCart(int id);
        Task UpdateCartWhenLogin(int UserId, string RandomNumber);
        Task DeleteCartByUserId(int id);
    }
}
