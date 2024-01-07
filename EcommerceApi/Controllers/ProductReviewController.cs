using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/ProductReview")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewRepository _dbproductReviewRepository;
        protected ApiResponse _response;
        
        public ProductReviewController(IProductReviewRepository dbproductReviewRepository)
        {
            _dbproductReviewRepository = dbproductReviewRepository;
            this._response = new ApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateProductReview([FromBody] ProductReviewDTO productReviewDTO)
        {
            try
            {
                if (productReviewDTO == null)
                {
                    return BadRequest("productReview data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var model = new ProductReview()
                {
                    UserId=productReviewDTO.UserId,
                    ProductId=productReviewDTO.ProductId,
                    ReviewDescription=productReviewDTO.ReviewDescription,
                    Rating=productReviewDTO.Rating,
                    ReviewDate=DateTime.Now,
                    IsActive=true
                };

                bool result = await _dbproductReviewRepository.CreateProductReview(model);
                if (result == false)
                {
                    ModelState.AddModelError("ProductReview", "You have already reviewed this product");
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

        [HttpGet("{Productid:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetProductReviewByProductId(int Productid)
        {
            try
            {
                if (Productid <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var result = await _dbproductReviewRepository.GetProductReviewByProductId(Productid);
                _response.Result = result;
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
