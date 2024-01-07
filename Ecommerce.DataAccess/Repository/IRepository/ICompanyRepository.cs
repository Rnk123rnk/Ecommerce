using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository
    {
        Task<List<Models.Models.Company>> GetCompanyList();
        Task<Models.Models.Company> GetCompanyById(int id);
        Task<bool> CreateCompany(Models.Models.Company obj);
        Task<bool> DeleteCompany(int id);
        Task<int> UpdateCompany(Models.Models.Company obj);
        Task<bool> CompanyIdExit(int id);
    }
}
