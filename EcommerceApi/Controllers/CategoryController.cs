using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EcommerceApi.Controllers
{
    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _dbCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected ApiResponse _response;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dbCategoryRepository = categoryRepository;
            this._response = new ApiResponse();
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateCategory([FromForm] CategoryWithImageDTO categoryImageDTO)
        {
            try
            {
                if(categoryImageDTO == null)
                {
                    return BadRequest("No Data");
                }

                if(await _dbCategoryRepository.CategordNameExit(categoryImageDTO.CategoryName) ==true)
                {
                    ModelState.AddModelError("Category", "Category already exists");
                    return Conflict(ModelState);
                }
                string UrlPathImg = "";
                if (categoryImageDTO.CategoryImage != null)
                {
                    var imageFile = categoryImageDTO.CategoryImage;
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", uniqueFileName);
                    var httpRequest = _httpContextAccessor.HttpContext.Request;
                     UrlPathImg = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{uniqueFileName}";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await categoryImageDTO.CategoryImage.CopyToAsync(fileStream);
                    }
                }
                var category = new Category
                {
                    CategoryName = categoryImageDTO.CategoryName,
                    IsActive = categoryImageDTO.IsActive,
                    UpdateDate = DateTime.Now,
                };
                if(UrlPathImg != null)
                {
                    category.CategoryImage = UrlPathImg;
                }
                await _dbCategoryRepository.CreateCategory(category);
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

        [HttpPost("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> UpdateCategory([FromForm] CategoryUploadImgDTO categoryImageDTO)
        {
            try
            {
                var category = new Category
                {
                    CategoryId = Convert.ToInt32(categoryImageDTO.CategoryId),
                    CategoryName = categoryImageDTO.CategoryName,
                    IsActive = categoryImageDTO.IsActive,
                    UpdateDate = DateTime.Now
                };
                if (categoryImageDTO.CategoryImage != null)
                {

                    var imageFile = categoryImageDTO.CategoryImage;

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", uniqueFileName);
                    var httpRequest = _httpContextAccessor.HttpContext.Request;
                    var UrlPathImg = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{uniqueFileName}";
                    category.CategoryImage = UrlPathImg;
                    int updateResult = await _dbCategoryRepository.UpdateCategory(category);
                    if (updateResult == 1)
                    {
                        ModelState.AddModelError("Category", "Category already exists!");
                        return Conflict(ModelState);
                    }
                    if (updateResult == 2)
                    {
                        return NotFound($"Category with Id {categoryImageDTO.CategoryId} not found.");
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await categoryImageDTO.CategoryImage.CopyToAsync(fileStream);
                    }
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(categoryImageDTO.OldCategoryImage))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", categoryImageDTO.OldCategoryImage);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }
                else
                {
                    category.CategoryImage = categoryImageDTO.OldCategoryImage;
                    int updateResult = await _dbCategoryRepository.UpdateCategory(category);
                    if (updateResult == 1)
                    {
                        ModelState.AddModelError("Category", "Category already exists!");
                        return Conflict(ModelState);
                    }
                    if (updateResult == 2)
                    {
                        return NotFound($"Category with Id {categoryImageDTO.CategoryId} not found.");
                    }
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetCategoryList()
        {
            try
            {
                IEnumerable<Category> category = await _dbCategoryRepository.GetCategoryList();
                _response.Result = category;
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

        [HttpGet("MenuFill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>>GetMenuList()
        {
            try
            {
                var categories = await _dbCategoryRepository.MenuFill();
                var categoryDTOs = categories.Select(category => new Category
                {
                    CategoryName = category.CategoryName,
                     SubCategories= category.SubCategories.Select(subCategory => new SubCategory
                    {
                        SubCategoryName = subCategory.SubCategoryName,
                        ThirdCategories = subCategory.ThirdCategories.Select(thirdCategory => new ThirdCategory
                        {
                            ThirdCategoryName = thirdCategory.ThirdCategoryName
                        }).ToList()
                    }).ToList()
                }).ToList();
                _response.Result = categoryDTOs;
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
                return null;
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCategoryById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var category = await _dbCategoryRepository.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound("Category not exists Please check id");
                }
                _response.Result = category;
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool CategoryExists = await _dbCategoryRepository.DeleteCategory(id);
                if (!CategoryExists)
                {
                    return NotFound("Category not exists Please check id");
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
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
