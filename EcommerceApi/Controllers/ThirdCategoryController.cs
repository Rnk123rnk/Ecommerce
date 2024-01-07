using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/ThirdCategoryAPI")]
    [ApiController]
    public class ThirdCategoryController : ControllerBase
    {
        private readonly IThirdCategoryRepository _dbIthirdCategoryRepository;
        private readonly ICategoryRepository _dbICategoryRepository;
        private readonly ISubCategoryRepository _dbISubCategoryRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public ThirdCategoryController(IThirdCategoryRepository dbIthirdCategoryRepository,ISubCategoryRepository dbISubCategoryRepository,ICategoryRepository dbIcategoryRepository, IMapper mapper)
        {
            _dbIthirdCategoryRepository = dbIthirdCategoryRepository;
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
        public async Task<ActionResult<ApiResponse>> CreateThirdCategory([FromBody] ThirdCategoryDTO thirdcategoryDTO)
        {
            try
            {
                if (thirdcategoryDTO == null)
                {
                    return BadRequest("ThirdCategory data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _dbICategoryRepository.CategoryIdExit(thirdcategoryDTO.CategoryId) == false)
                {
                    return NotFound($"CategoryId {thirdcategoryDTO.CategoryId} not found in category table");
                }
                if (await _dbISubCategoryRepository.SubCategoryIdExit(thirdcategoryDTO.SubCategoryId) == false)
                {
                    return NotFound($"SubCategoryId {thirdcategoryDTO.CategoryId} not found in subcategory table");
                }

                ThirdCategory model = _mapper.Map<ThirdCategory>(thirdcategoryDTO);
                model.UpdateDate = DateTime.Now;

                bool result = await _dbIthirdCategoryRepository.CreateThirdCategory(model);

                if (result == false)
                {
                    ModelState.AddModelError("ThirdCategory", "ThirdCategory already exists");
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
        public async Task<ActionResult<ApiResponse>> UpdateThirdCategory(int id, [FromBody] ThirdCategoryDTO thirdcategoryDTO)
        {
            try
            {
                if (thirdcategoryDTO == null || id <= 0)
                {
                    return BadRequest("Invaild data or id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _dbICategoryRepository.CategoryIdExit(thirdcategoryDTO.CategoryId) == false)
                {
                    return NotFound($"CategoryId {thirdcategoryDTO.CategoryId} not found in category table");
                }
                if (await _dbISubCategoryRepository.SubCategoryIdExit(thirdcategoryDTO.SubCategoryId) == false)
                {
                    return NotFound($"SubCategoryId {thirdcategoryDTO.CategoryId} not found in subcategory table");
                }

                ThirdCategory model = _mapper.Map<ThirdCategory>(thirdcategoryDTO);
                model.ThirdCategoryId = id;
                model.UpdateDate = DateTime.Now;
               
                int UpdateResilt = await _dbIthirdCategoryRepository.UpdateThirdCategory(model);

                if (UpdateResilt == 1)
                {
                    ModelState.AddModelError("ThirdCategory", "ThirdCategory already exists!");
                    return Conflict(ModelState);
                }
                if (UpdateResilt == 2)
                {
                    return NotFound($"ThirdCategory with Id {id} not found.");
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
        public async Task<ActionResult<ApiResponse>> GetThirdCategoryList([FromQuery(Name = "FilterId")] int? Id)
        {
            try
            {
                if (Id.HasValue && Id.Value > 0)
                {
                    IEnumerable<ThirdCategory> thirdCategories;
                    thirdCategories = await _dbIthirdCategoryRepository.GetByIdThirdCategoryList(Id.Value);
                    if (!thirdCategories.Any())
                    {
                        ModelState.AddModelError("Id", "Id does not exist in Table!");
                        return BadRequest(ModelState);
                    }
                    _response.Result = thirdCategories;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    IEnumerable<ThirdCategoryListDTO> subcategory = await _dbIthirdCategoryRepository.GetThirdCategoryList();
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
        public async Task<ActionResult<ApiResponse>> GetThirdCategoryById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var ThirdcategoryExists = await _dbIthirdCategoryRepository.GeThirdCategoryById(id);
                if (ThirdcategoryExists == null)
                {
                    return NotFound("ThirdCategory not exists Please check id");
                }
                _response.Result = ThirdcategoryExists;
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
        public async Task<ActionResult<ApiResponse>> DeleteThirdCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool thirdcategoryExists = await _dbIthirdCategoryRepository.DeleteThirdCategory(id);
                if (!thirdcategoryExists)
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
