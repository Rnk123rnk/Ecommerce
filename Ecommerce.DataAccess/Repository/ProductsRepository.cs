using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecommerce.DataAccess.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateProduct(Product obj)
        {
            if (await _db.Products.FirstOrDefaultAsync(u => u.ProductName == obj.ProductName) == null)
            {
                await _db.Products.AddAsync(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> ProductExit(string Name)
        {
            if (await _db.Products.FirstOrDefaultAsync(u => u.ProductName == Name) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> ProductIdExit(int id)
        {
            if (await _db.Products.FirstOrDefaultAsync(u => u.ProductId == id) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<SingleProductForClientDTO> GetProductByName(string ProductName)
        {
            var product = await _db.Products
        .Where(product => product.IsActive && product.ProductName == ProductName)
        .GroupJoin(
            _db.ProductReviews,
            product => product.ProductId,
            review => review.ProductId,
            (product, reviews) => new SingleProductForClientDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage1 = product.ProductImage1,
                ProductImage2 = product.ProductImage2,
                ProductImage3 = product.ProductImage3,
                ProductImage4 = product.ProductImage4,
                ProductDiscountedPrice = product.ProductDiscountedPrice,
                ProductMRPPrice = product.ProductMRPPrice,
                Quantity = product.Quantity,
                OneStarCount = reviews.Count(r => r.Rating == 1),
                TwoStarCount = reviews.Count(r => r.Rating == 2),
                ThreeStarCount = reviews.Count(r => r.Rating == 3),
                FourStarCount = reviews.Count(r => r.Rating == 4),
                FiveStarCount = reviews.Count(r => r.Rating == 5),
                TotalReviews = reviews.Count(),
                AverageRating = (int)(reviews.Any() ? reviews.Average(r => r.Rating) : 0),
            })
        .FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<Product>> GetProductList(int select, int skipitem)
        {
            var productList = await _db.Products.OrderBy(p => p.ProductId).Skip(skipitem).Take(select).ToListAsync();
            return productList;
        }
        public async Task<List<ProductListForClientDTO>> ProductByCategoryName(string Name, string categoryName, string SortBy, int PageNumber)
        {
            var itemsPerPage = 12;
            var skipCount = itemsPerPage * (PageNumber - 1);
            if (categoryName == "ThirdCategory")
            {
                var query = _db.Products
                    .Where(product => product.IsActive)
                    .Join(
                        _db.ThirdCategorys,
                        product => product.ThirdCategoryId,
                        thirdcategory => thirdcategory.ThirdCategoryId,
                        (product, thirdcategory) => new { Product = product, Thirdcategory = thirdcategory }
                    )
                    .Join(
                        _db.Categorys,
                        joined => joined.Thirdcategory.CategoryId,
                        category => category.CategoryId,
                        (joined, category) => new { Product = joined.Product, Thirdcategory = joined.Thirdcategory, Category = category }
                    )
                    .Where(joined => joined.Thirdcategory.ThirdCategoryName == Name)
                    .GroupJoin(
                        _db.ProductReviews,
                        joined => joined.Product.ProductId,
                        review => review.ProductId,
                        (joined, reviewGroup) => new
                        {
                            Product = joined.Product,
                            CategoryName = joined.Category.CategoryName,
                            TotalReviews = reviewGroup.Count(),
                            AverageRating = reviewGroup.Any() ? (int)reviewGroup.Average(r => r.Rating) : 0
                        }
                    )
                    .Select(joined => new ProductListForClientDTO
                    {
                        ProductId = joined.Product.ProductId,
                        ProductName = joined.Product.ProductName,
                        ProductImage1 = joined.Product.ProductImage1,
                        ProductDiscountedPrice = joined.Product.ProductDiscountedPrice,
                        ProductMRPPrice = joined.Product.ProductMRPPrice,
                        CategoryName = joined.CategoryName,
                        AverageRating = joined.AverageRating,
                        TotalReviews = joined.TotalReviews,
                    });
                switch (SortBy)
                {
                    case "New":
                        query = query.OrderByDescending(p => p.ProductId);
                        break;
                    case "LowHigh":
                        query = query.OrderBy(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "HighLow":
                        query = query.OrderByDescending(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "A-Z":
                        query = query.OrderBy(p => p.ProductName);
                        break;
                    case "Z-A":
                        query = query.OrderByDescending(p => p.ProductName);
                        break;
                }
                var products = await query
                    .Skip(skipCount)
                    .Take(itemsPerPage)
                    .ToListAsync();
                return products;
            }
            else if (categoryName == "SubCategory")
            {
                var query = _db.Products
                .Where(product => product.IsActive)
                .Join(
                    _db.SubCategorys,
                    product => product.SubCategoryId,
                    subcategory => subcategory.SubCategoryId,
                    (product, subcategory) => new { Product = product, Subcategory = subcategory }
                )
                .Join(
                    _db.Categorys,
                    joined => joined.Subcategory.CategoryId,
                    category => category.CategoryId,
                    (joined, category) => new { Product = joined.Product, Subcategory = joined.Subcategory, Category = category }
                )
                .Where(joined => joined.Subcategory.SubCategoryName == Name)
                .GroupJoin(
                    _db.ProductReviews,
                    joined => joined.Product.ProductId,
                    review => review.ProductId,
                    (joined, reviewGroup) => new
                    {
                        Product = joined.Product,
                        CategoryName = joined.Category.CategoryName,
                        TotalReviews = reviewGroup.Count(),
                        AverageRating = reviewGroup.Any() ? (int)reviewGroup.Average(r => r.Rating) : 0
                    }
                )
                .Select(joined => new ProductListForClientDTO
                {
                    ProductId = joined.Product.ProductId,
                    ProductName = joined.Product.ProductName,
                    ProductImage1 = joined.Product.ProductImage1,
                    ProductDiscountedPrice = joined.Product.ProductDiscountedPrice,
                    ProductMRPPrice = joined.Product.ProductMRPPrice,
                    CategoryName = joined.CategoryName,
                    TotalReviews = joined.TotalReviews,
                    AverageRating = joined.AverageRating,
                });
                switch (SortBy)
                {
                    case "New":
                        query = query.OrderByDescending(p => p.ProductId);
                        break;
                    case "LowHigh":
                        query = query.OrderBy(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "HighLow":
                        query = query.OrderByDescending(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "A-Z":
                        query = query.OrderBy(p => p.ProductName);
                        break;
                    case "Z-A":
                        query = query.OrderByDescending(p => p.ProductName);
                        break;
                }
                var products = await query
                    .Skip(skipCount)
                    .Take(itemsPerPage)
                    .ToListAsync();
                return products;
            }
            else if (categoryName == "Category")
            {
                var query = _db.Products.Where(product => product.IsActive)
                                                 .Join(_db.Categorys,
                                                  product => product.CategoryId,
                                                  category => category.CategoryId,
                               (product, category) => new { Product = product, Category = category })
                                                  .Where(joined => joined.Category.CategoryName == Name)
                                                  .GroupJoin(
                                                  _db.ProductReviews,
                                                   joined => joined.Product.ProductId,
                                                     review => review.ProductId,
                                (joined, reviewGroup) => new
                                {
                                    Product = joined.Product,
                                    CategoryName = joined.Category.CategoryName,
                                    AverageRating = reviewGroup.Any() ? (int)reviewGroup.Average(r => r.Rating) : 0,
                                    TotalReviews = reviewGroup.Count(),

                                }
                            )
                            .Select(joined => new ProductListForClientDTO
                            {
                                ProductId = joined.Product.ProductId,
                                ProductName = joined.Product.ProductName,
                                ProductImage1 = joined.Product.ProductImage1,
                                ProductDiscountedPrice = joined.Product.ProductDiscountedPrice,
                                ProductMRPPrice = joined.Product.ProductMRPPrice,
                                CategoryName = joined.CategoryName,
                                AverageRating = joined.AverageRating,
                                TotalReviews = joined.TotalReviews,
                            });
                switch (SortBy)
                {
                    case "New":
                        query = query.OrderByDescending(p => p.ProductId);
                        break;
                    case "LowHigh":
                        query = query.OrderBy(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "HighLow":
                        query = query.OrderByDescending(p => p.ProductMRPPrice - p.ProductDiscountedPrice);
                        break;
                    case "A-Z":
                        query = query.OrderBy(p => p.ProductName);
                        break;
                    case "Z-A":
                        query = query.OrderByDescending(p => p.ProductName);
                        break;
                }
                var products = await query
                    .Skip(skipCount)
                    .Take(itemsPerPage)
                    .ToListAsync();
                return products;
            }
            else
            {
                throw new ArgumentException("Invalid categoryName");
            }
        }

        public async Task<ForProductPage> ProductPageDetali(string Name, string CategoryName)
        {
            if (CategoryName == "ThirdCategory")
            {
                var productPageDetail = await _db.Products
                   .Where(product => product.IsActive)
                   .Join(
                       _db.ThirdCategorys,
                       product => product.ThirdCategoryId,
                       thirdcategory => thirdcategory.ThirdCategoryId,
                       (product, thirdcategory) => new { Product = product, Thirdcategory = thirdcategory }
                   )
                   .Where(joined => joined.Thirdcategory.ThirdCategoryName == Name)
                   .GroupBy(joined => joined.Thirdcategory.ThirdCategoryName)
                   .Select(group => new ForProductPage
                   {
                       ProductPageMaxPrice = group.Max(p => p.Product.ProductMRPPrice),
                       ProductPageMinPrice = group.Min(p => p.Product.ProductMRPPrice),
                       TotalProductCount = group.Count()
                   })
                   .FirstOrDefaultAsync();
                return productPageDetail;
            } else if(CategoryName == "SubCategory")
                {
                    var productPageDetail = await _db.Products
                        .Where(product => product.IsActive)
                        .Join(
                            _db.SubCategorys,
                            product => product.SubCategoryId,
                            subcategory => subcategory.SubCategoryId,
                            (product, subcategory) => new { Product = product, Subcategory = subcategory }
                        )
                        .Where(joined => joined.Subcategory.SubCategoryName == Name)
                        .GroupBy(joined => joined.Subcategory.SubCategoryName)
                        .Select(group => new ForProductPage
                        {
                            ProductPageMaxPrice = group.Max(p => p.Product.ProductMRPPrice),
                            ProductPageMinPrice = group.Min(p => p.Product.ProductMRPPrice),
                            TotalProductCount = group.Count()
                        })
                        .FirstOrDefaultAsync();

                    return productPageDetail;
            }
            else if(CategoryName == "Category")
            {
                var productPageDetail = await _db.Products
                   .Where(product => product.IsActive)
                   .Join(
                        _db.Categorys,
                        product => product.CategoryId,
                        category => category.CategoryId,
                        (product, category) => new { Product = product, Category = category }
                   )
                   .Where(joined => joined.Category.CategoryName == Name)
                   .GroupBy(joined => joined.Category.CategoryName)
                   .Select(group => new ForProductPage
                   {
                       ProductPageMaxPrice = group.Max(p => p.Product.ProductMRPPrice),
                       ProductPageMinPrice = group.Min(p => p.Product.ProductMRPPrice),
                       TotalProductCount = group.Count()
                   })
                    .FirstOrDefaultAsync();
                return productPageDetail;
            }
            else
            {
                throw new ArgumentException("Invalid categoryName");
            }

        }
        public async Task<List<Product>> ProductByName(string Name)
        {
            return await _db.Products.Where(u => EF.Functions.Like(u.ProductName, $"%{Name}%")).ToListAsync();
        }


        public async Task<int> UpdateProduct(Product obj)
        {
            if (await _db.Products.FirstOrDefaultAsync(u => obj.ProductName == u.ProductName && u.ProductId != obj.ProductId) != null)
            {
                return 1;
            }
            var products = await _db.Products.FirstOrDefaultAsync(u => obj.ProductId == u.ProductId);
            if (products == null)
            {
                return 2;
            }
            _db.Entry(products).CurrentValues.SetValues(obj);
            await _db.SaveChangesAsync();
            return 3;
        }
    }
}
