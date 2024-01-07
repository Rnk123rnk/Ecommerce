

using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IUsersRepository
    {
        Task<List<Users>> GetUsers();
        Task<Users> GetUser(int id);
        Task<int> CreateUser(Users user);
        Task<bool> DeleteUser(int id);
        Task<int> UpdateUser(Users user);
        Task<LogInResponseDTO> LogIn(LogInDTO logInDTO);
    }
}
