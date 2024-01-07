using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IProductSpecsRepository
    {
        Task<ProductSpecs> GetProductSpecsById(int id);
        Task<bool> CreateProductSpecs(ProductSpecs obj);
        Task<bool> DeleteProductSpecs(int id);
        Task<int> UpdateProductSpecs(ProductSpecs obj);
        Task<List<ProductSpecs>> GetProductSpecsByProductId(int id);
    }
}
