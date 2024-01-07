using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/SubCategoryAPI")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryRepository _dbISubCategoryRepository;
        private readonly ICategoryRepository _dbICategoryRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public SubCategoryController(ISubCategoryRepository dbISubCategoryRepository, ICategoryRepository dbIcategoryRepository, IMapper mapper)
        {
            _dbICategoryRepository = dbIcategoryRepository;
            _dbISubCategoryRepository = dbISubCategoryRepository;
            _mapper = mapper;
            this._response = new ApiResponse();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CreateSubCategory([FromBody] SubCategoryDTO subcategoryDTO)
        {
            try
            {
                if (subcategoryDTO == null)
                {
                    return BadRequest("SubCategory data is null");
                }
          
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _dbICategoryRepository.CategoryIdExit(subcategoryDTO.CategoryId) == false)
                {
                    return NotFound($"CategoryId {subcategoryDTO.CategoryId} not found in category table");
                }

                SubCategory model = _mapper.Map<SubCategory>(subcategoryDTO);
                model.UpdateDate = DateTime.Now;

      
                bool result = await _dbISubCategoryRepository.CreateSubCategory(model);
            
                if (result == false)
                {
                    ModelState.AddModelError("SubCategory", "SubCategory already exists");
                    return Conflict(ModelState);
                }
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> UpdateSubCategory(int id, [FromBody] SubCategoryDTO subcategoryDTO)
        {
            try
            {
                if (subcategoryDTO == null || id <= 0)
                {
                    return BadRequest("Invaild data or id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _dbICategoryRepository.CategoryIdExit(subcategoryDTO.CategoryId) == false)
                {
                    return NotFound($"CategoryId {subcategoryDTO.CategoryId} not found in category table");
                }

                SubCategory model = _mapper.Map<SubCategory>(subcategoryDTO);
                model.SubCategoryId = id;
                model.UpdateDate = DateTime.Now;

                int UpdateResilt = await _dbISubCategoryRepository.UpdateSubCategory(model);
             
                if (UpdateResilt == 1)
                {
                    ModelState.AddModelError("SubCategory", "SubCategory already exists!");
                    return Conflict(ModelState);
                }
                if (UpdateResilt == 2)
                {
                    return NotFound($"SubCategory with Id {id} not found.");
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
        public async Task<ActionResult<ApiResponse>> GetSubCategoryList([FromQuery(Name = "FilterId")] int? Id)
        {
            try
            {
                if (Id.HasValue && Id.Value > 0)
                {
                    IEnumerable<SubCategory> subcategory;
                    subcategory = await _dbISubCategoryRepository.GetByIdSubCategoryList(Id.Value);
                    if (!subcategory.Any())
                    {
                        ModelState.AddModelError("Id", "Id does not exist in Table!");
                        return BadRequest(ModelState);
                    }
                    _response.Result = subcategory;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    IEnumerable<SubCategoryListDTO> subcategory;
                    subcategory = await _dbISubCategoryRepository.GetSubCategoryList();
                    _response.Result = subcategory;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
    
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


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetSubCategoryById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var subcategoryExists = await _dbISubCategoryRepository.GetSubCategoryById (id);
                if (subcategoryExists == null)
                {
                    return NotFound("SubCategory not exists Please check id");
                }
                _response.Result = subcategoryExists;
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
        public async Task<ActionResult<ApiResponse>> DeleteSubCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool subcategoryExists = await _dbISubCategoryRepository.DeleteSubCategory(id);
                if (!subcategoryExists)
                {
                    return NotFound("SubCategory not exists Please check id");
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
