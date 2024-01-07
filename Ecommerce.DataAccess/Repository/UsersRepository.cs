
using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.DataAccess.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        private string secrekey;
        public UsersRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secrekey = configuration["ApiSetting:Secret"];
        }
        public async Task<int> CreateUser(Users user)
        {
            if (await _db.Userss.FirstOrDefaultAsync(u => u.MobileNumber == user.MobileNumber) != null)
            {
                return 1;
            }
          await  _db.Userss.AddAsync(user);
            await _db.SaveChangesAsync();
            return 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _db.Userss.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                _db.Userss.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<Users> GetUser(int id)
        {
            return await _db.Userss.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<Users>> GetUsers()
        {
            return await _db.Userss.ToListAsync();
        }

        public async Task<int> UpdateUser(Users user)
        {
                if (await _db.Userss.FirstOrDefaultAsync(u => u.MobileNumber == user.MobileNumber && u.UserId != user.UserId) != null)
                {
                    return 1;
                }

                var existingUser = await _db.Userss.FirstOrDefaultAsync(u => u.UserId == user.UserId);

                if (existingUser == null)
                {
                    return 2;

                }
                _db.Entry(existingUser).CurrentValues.SetValues(user);
                await _db.SaveChangesAsync();
                return 3;
        }

        public async Task<LogInResponseDTO> LogIn(LogInDTO logInDTO)
        {
            var user = await _db.Userss.FirstOrDefaultAsync(u => u.MobileNumber == logInDTO.MobileNumber && u.Password == logInDTO.Password);

            if (user == null)
            {
                return new LogInResponseDTO()
                {
                    Token = "",
                    UserId = 0
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secrekey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserId.ToString()),
                    new Claim(ClaimTypes.Role,user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            LogInResponseDTO logInResponseDTO = new LogInResponseDTO();
            logInResponseDTO.Token = tokenString;
            logInResponseDTO.UserId = user.UserId;
            logInResponseDTO.role = user.Role;
            return logInResponseDTO;
        }
    }
}
