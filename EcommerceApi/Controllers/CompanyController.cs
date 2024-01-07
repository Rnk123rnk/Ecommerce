using AutoMapper;
using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/CompanyAPI")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _dbcompanyRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public CompanyController(ICompanyRepository dbcompanyRepository, IMapper mapper)
        {
            _dbcompanyRepository = dbcompanyRepository;
            this._response = new ApiResponse();
            _mapper = mapper;   
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateCompany([FromBody] CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO == null)
                {
                    return BadRequest("Company data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Company model = _mapper.Map<Company>(companyDTO);
                model.UpdateDate = DateTime.Now;

                bool result = await _dbcompanyRepository.CreateCompany(model);
                if (result == false)
                {
                    ModelState.AddModelError("Company", "Company already exists");
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
        public async Task<ActionResult<ApiResponse>> UpdateCompany(int id, [FromBody] CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO == null || id <= 0)
                {
                    return BadRequest("Invaild data or id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Company model = _mapper.Map<Company>(companyDTO);
                model.CompanyId = id;
                model.UpdateDate = DateTime.Now;

                int UpdateResult = await _dbcompanyRepository.UpdateCompany(model);

                if (UpdateResult == 1)
                {
                    ModelState.AddModelError("Company", "Company already exists");
                    return Conflict(ModelState);
                }
                if (UpdateResult == 2)
                {
                    return NotFound($"Company with Id {id} not found.");
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
        public async Task<ActionResult<ApiResponse>> GetCompanyList()
        {
            try
            {
                IEnumerable<Company>company = await _dbcompanyRepository.GetCompanyList();
                _response.Result = company;
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


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCompanyById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var company = await _dbcompanyRepository.GetCompanyById(id);
                if (company == null)
                {
                    return NotFound("company not exists Please check id");
                }
                _response.Result = company;
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
        public async Task<ActionResult<ApiResponse>> DeleteCompany(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool CompanyExists = await _dbcompanyRepository.DeleteCompany(id);
                if (!CompanyExists)
                {
                    return NotFound("Company not exists Please check id");
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
