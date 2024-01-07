using AutoMapper;
using Azure;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IThirdCategoryRepository _dbIthirdCategoryRepository;
        private readonly ICategoryRepository _dbICategoryRepository;
        private readonly ISubCategoryRepository _dbISubCategoryRepository;
        private readonly IProductsRepository _dbIIProductsRepository;
        private readonly ImageUpload _imageUpload;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public ProductController(IThirdCategoryRepository dbIthirdCategoryRepository, ICategoryRepository dbICategoryRepository, ISubCategoryRepository dbISubCategoryRepository, IProductsRepository dbIIProductsRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _dbIthirdCategoryRepository = dbIthirdCategoryRepository;
            _dbICategoryRepository = dbICategoryRepository;
            _dbISubCategoryRepository = dbISubCategoryRepository;
            _dbIIProductsRepository = dbIIProductsRepository;
            _mapper = mapper;
            _imageUpload = new ImageUpload(webHostEnvironment, httpContextAccessor);
            this._response = new ApiResponse();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateProduct([FromForm] ProductDTO productDTO)
        {
            try
            {
                if (productDTO == null)
                {
                    return BadRequest("Product data is null");
                }
                if (await _dbIIProductsRepository.ProductExit(productDTO.ProductName) == true)
                {
                    ModelState.AddModelError("Product", "Product already exists");
                    return Conflict(ModelState);
                }
                var imageUrl = await _imageUpload.CreateImage(productDTO.ProductImage1, productDTO.ProductImage2, productDTO.ProductImage3, productDTO.ProductImage4);

                var SetmModel = new Product()
                {
                    CategoryId = productDTO.CategoryId,
                    SubCategoryId = productDTO.SubCategoryId,
                    ThirdCategoryId = productDTO.ThirdCategoryId,
                    CompanyId = productDTO.CompanyId,
                    ProductName = productDTO.ProductName,
                    IsActive = productDTO.IsActive,
                    ProductMRPPrice = productDTO.ProductMRPPrice,
                    Quantity = productDTO.Quantity,
                    CreatedDate = DateTime.Now,
                };
                if (productDTO.ProductDiscountedPrice > 0)
                {
                    SetmModel.ProductDiscountedPrice = productDTO.ProductDiscountedPrice;

                }
                // Assign file paths if they exist
                if (productDTO.ProductImage1 != null)
                {
                    SetmModel.ProductImage1 = imageUrl.FirstOrDefault(p => p.Id == 1)?.Path;
                }
                if (productDTO.ProductImage2 != null)
                {
                    SetmModel.ProductImage2 = imageUrl.FirstOrDefault(p => p.Id == 2)?.Path;
                }
                if (productDTO.ProductImage3 != null)
                {
                    SetmModel.ProductImage3 = imageUrl.FirstOrDefault(p => p.Id == 3)?.Path;
                }
                if (productDTO.ProductImage4 != null)
                {
                    SetmModel.ProductImage4 = imageUrl.FirstOrDefault(p => p.Id == 3)?.Path;
                }
                await _dbIIProductsRepository.CreateProduct(SetmModel);
                return Ok();
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("ProductList/{getitem:int}/{skipitem:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductList(int getitem, int skipitem, [FromQuery(Name = "ProductName")] string? ProductName)
        {
            try
            {
                IEnumerable<Product> products;

                if (!string.IsNullOrEmpty(ProductName))
                {
                    products = await _dbIIProductsRepository.ProductByName(ProductName);

                    if (!products.Any())
                    {
                        ModelState.AddModelError("ProductName", "No products found with the provided name.");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    products = await _dbIIProductsRepository.GetProductList(getitem, skipitem);
                }
                _response.Result = products;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("ProductListClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductList([FromQuery(Name = "Category")] string? Category, [FromQuery(Name = "SubCategory")] string? SubCategory, [FromQuery(Name = "ThirdCategory")] string? ThirdCategory, [FromQuery(Name = "SortBy")] string? SortBy, [FromQuery(Name = "PageNumber")] int PageNumber)
        {
            try
            {
                if(SortBy == null)
                {
                    return BadRequest("SortBy Is Required ex: New || LowHight || HightLow || A-Z || Z-A And PageNumber 1");
                }
                string categoryName = "";
                string categoryType = "";

                if (!string.IsNullOrEmpty(Category))
                {
                    categoryName = Category;
                    categoryType = "Category";
                }
                else if (!string.IsNullOrEmpty(SubCategory))
                {
                    categoryName = SubCategory;
                    categoryType = "SubCategory";
                }
                else if (!string.IsNullOrEmpty(ThirdCategory))
                {
                    categoryName = ThirdCategory;
                    categoryType = "ThirdCategory";
                }
                else
                {
                    ModelState.AddModelError("Category", "No category parameter provided.");
                    return BadRequest(ModelState);
                }

                var products = await _dbIIProductsRepository.ProductByCategoryName(categoryName, categoryType, SortBy, PageNumber);

                if (products == null)
                {
                    ModelState.AddModelError("ProductList", "No products found with the provided category name.");
                    return BadRequest(ModelState);
                }

                _response.Result = products;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("ProductPageFilterClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductListCountAndprice([FromQuery(Name = "Category")] string? Category, [FromQuery(Name = "SubCategory")] string? SubCategory, [FromQuery(Name = "ThirdCategory")] string? ThirdCategory)
        {
            try
            {
                string categoryName = "";
                string categoryType = "";

                if (!string.IsNullOrEmpty(Category))
                {
                    categoryName = Category;
                    categoryType = "Category";
                }
                else if (!string.IsNullOrEmpty(SubCategory))
                {
                    categoryName = SubCategory;
                    categoryType = "SubCategory";
                }
                else if (!string.IsNullOrEmpty(ThirdCategory))
                {
                    categoryName = ThirdCategory;
                    categoryType = "ThirdCategory";
                }
                else
                {
                    ModelState.AddModelError("Category", "No category parameter provided.");
                    return BadRequest(ModelState);
                }

                var products = await _dbIIProductsRepository.ProductPageDetali(categoryName, categoryType);

                if (products == null)
                {
                    ModelState.AddModelError("ProductList", "No products found with the provided category name.");
                    return BadRequest(ModelState);
                }

                _response.Result = products;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpGet("ProductListClient/{productname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductList(string productname)
        {
            try
            {

                var products = await _dbIIProductsRepository.GetProductByName(productname);

                if (products == null)
                {
                    ModelState.AddModelError("ProductList", "No products found with the provided category name.");
                    return BadRequest(ModelState);
                }

                _response.Result = products;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

    }
}
