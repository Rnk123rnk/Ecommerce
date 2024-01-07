
using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccess.Repository
{
    public class UserPersonalAddresseRepository : IUserPersonalAddresseRepository
    {
        private readonly ApplicationDbContext _db;

        public UserPersonalAddresseRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateUserPersonalAddresse(UserPersonalAddresse user)
        {
            if (await _db.Userss.FirstOrDefaultAsync(u => u.UserId == user.UserId) == null)
            {
                return false;
            }
            await _db.UserPersonalAddresses.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserPersonalAddresse(int id)
        {
            var user = _db.UserPersonalAddresses.FirstOrDefault(u => u.UserPersonalAddresseId == id);
            if (user != null)
            {
                _db.UserPersonalAddresses.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<UserPersonalAddresse>> GetUserPersonalAddresses()
        {
            return await _db.UserPersonalAddresses.ToListAsync();
        }

        public async Task<List<UserPersonalAddresse>> GetUserPersonalAddresses(int id)
        {
            return await _db.UserPersonalAddresses
                               .Where(u => u.UserId == id).ToListAsync();
        }
        public async Task<UserPersonalAddresse> GetUserPersonalAddresse(int id)
        {
            return await _db.UserPersonalAddresses.FirstOrDefaultAsync(u => u.UserPersonalAddresseId == id);
        }

        public async Task<int> UpdateUserPersonalAddresse(UserPersonalAddresse user)
        {
            if (await _db.Userss.FirstOrDefaultAsync(u => u.UserId == user.UserId) == null)
            {
                return 1;
            }

            var existingUser = await _db.UserPersonalAddresses.FirstOrDefaultAsync(u => u.UserPersonalAddresseId == user.UserPersonalAddresseId);

            if (existingUser == null)
            {
                return 2;

            }
            _db.Entry(existingUser).CurrentValues.SetValues(user);
            await _db.SaveChangesAsync();
            return 3;
        }
    }
}
