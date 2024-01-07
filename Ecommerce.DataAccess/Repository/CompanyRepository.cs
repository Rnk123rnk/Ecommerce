using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CompanyIdExit(int id)
        {
            if (await _db.Companys.FirstOrDefaultAsync(u => u.CompanyId == id) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateCompany(Models.Models.Company obj)
        {
            if (await _db.Companys.FirstOrDefaultAsync(u => u.CompanyName == obj.CompanyName) == null)
            {
                await _db.Companys.AddAsync(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCompany(int id)
        {
            var companys = await _db.Companys.FirstOrDefaultAsync(u => u.CompanyId == id);
            if (companys != null)
            {
                _db.Companys.Remove(companys);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _db.Companys.FirstOrDefaultAsync(u => u.CompanyId == id);

        }

        public async Task<List<Company>> GetCompanyList()
        {
            return await _db.Companys.ToListAsync();
        }

        public async Task<int> UpdateCompany(Models.Models.Company obj)
        {
            if (await _db.Companys.FirstOrDefaultAsync(u => obj.CompanyName == u.CompanyName && u.CompanyId != obj.CompanyId) != null)
            {
                return 1;
            }
            var companys = await _db.Companys.FirstOrDefaultAsync(u => obj.CompanyId == u.CompanyId);
            if (companys == null)
            {
                return 2;
            }
            _db.Entry(companys).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;
        }
    }
}
