using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProductList(int getitem, int skipitem);
        Task<Product> GetProductById(int id);
        Task<bool> CreateProduct(Product obj);
        Task<bool> DeleteProduct(int id);
        Task<int> UpdateProduct(Product obj);
        Task<bool> ProductIdExit(int id);
        Task<bool> ProductExit(string Name);
        Task<List<Product>> ProductByName(string Name);
        Task<SingleProductForClientDTO> GetProductByName(string ProductName);
        Task<ForProductPage> ProductPageDetali(string Name,string CategoryName);
        Task<List<ProductListForClientDTO>> ProductByCategoryName(string Name,string categoryName,string SortBy, int PageNumber);


    }
}
