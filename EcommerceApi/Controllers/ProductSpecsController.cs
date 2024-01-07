using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/ProductSpecsAPI")]
    [ApiController]
    public class ProductSpecsController : ControllerBase
    {
        private readonly IProductSpecsRepository _dbproductSpecsRepository;
        private readonly IProductsRepository _dbproductsRepository;

        protected ApiResponse _response;

        public ProductSpecsController(IProductSpecsRepository dbproductSpecsRepository, IProductsRepository dbproductsRepository)
        {
            _dbproductSpecsRepository = dbproductSpecsRepository;
            _dbproductsRepository = dbproductsRepository;
            this._response = new ApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CreateProductSpecs([FromBody] ProductSpecsDTO productSpecsDTO)
        {
            try  
            {
                if (productSpecsDTO == null)
                {
                    return BadRequest("productSpecs data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(await _dbproductsRepository.ProductIdExit(productSpecsDTO.ProductId) == false)
                {
                    return NotFound("ProductId Not Found");
                }

                var model = new ProductSpecs()
                {
                    ProductId = productSpecsDTO.ProductId,
                    SpecificationName = productSpecsDTO.SpecificationName,
                    SpecificationValue = productSpecsDTO.SpecificationValue
                };
                bool result = await _dbproductSpecsRepository.CreateProductSpecs(model);
                if (result == false)
                {
                    ModelState.AddModelError("productSpecs", "productSpecs already exists");
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
        public async Task<ActionResult<ApiResponse>> UpdateProductSpecs(int id, [FromBody] ProductSpecsDTO productSpecsDTO)
        {
            try
            {
                if (productSpecsDTO == null || id <= 0)
                {
                    return BadRequest("Invaild data or id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _dbproductsRepository.ProductIdExit(productSpecsDTO.ProductId) == false)
                {
                    return NotFound("ProductId Not Found");
                }
                var model = new ProductSpecs()
                {
                    ProductSpecsId = id,
                    ProductId = productSpecsDTO.ProductId,
                    SpecificationName = productSpecsDTO.SpecificationName,
                    SpecificationValue = productSpecsDTO.SpecificationValue
                };
                int UpdateResult = await _dbproductSpecsRepository.UpdateProductSpecs(model);

                if (UpdateResult == 1)
                {
                    ModelState.AddModelError("productSpecs", "productSpecs already exists");
                    return Conflict(ModelState);
                }
                if (UpdateResult == 2)
                {
                    return NotFound($"productSpecs with Id {id} not found.");
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


        [HttpGet("ProductListById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetProductSpecslistById(int id)
        {
            try
            {
                var Productspecs = await _dbproductSpecsRepository.GetProductSpecsByProductId(id);
                _response.Result = Productspecs;
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
        public async Task<ActionResult<ApiResponse>> GetProductspecsById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var productspecs = await _dbproductSpecsRepository.GetProductSpecsById(id);
                if (productspecs == null)
                {
                    return NotFound("ProductSpecs not exists Please check id");
                }
                _response.Result = productspecs;
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
        public async Task<ActionResult<ApiResponse>> DeleteProductSpec(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool CompanyExists = await _dbproductSpecsRepository.DeleteProductSpecs(id);
                if (!CompanyExists)
                {
                    return NotFound("ProductSpecs not exists Please check id");
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
