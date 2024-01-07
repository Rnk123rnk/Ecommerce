

using Ecommerce.Models.Models;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IUserPersonalAddresseRepository
    {
        Task<List<UserPersonalAddresse>> GetUserPersonalAddresses();
        Task<List<UserPersonalAddresse>> GetUserPersonalAddresses(int id);

        Task<UserPersonalAddresse> GetUserPersonalAddresse(int id);
        Task<bool> CreateUserPersonalAddresse(UserPersonalAddresse user);
        Task<bool> DeleteUserPersonalAddresse(int id);
        Task<int> UpdateUserPersonalAddresse(UserPersonalAddresse user);
    }
}
