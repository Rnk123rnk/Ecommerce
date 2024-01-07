using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcommerceApi.Controllers
{
    [Route("api/Wishlist")]
    [ApiController]
    public class ProductWishlistController : ControllerBase
    {
        private readonly IProductWishListRepository _dbproductWishListRepository;
        protected ApiResponse _response;

        public ProductWishlistController(IProductWishListRepository dbproductWishListRepository)
        {
            _dbproductWishListRepository = dbproductWishListRepository;
            this._response = new ApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateOrRemove([FromBody] ProductWishlistDTO productWishlist)
        {
            try
            {
                if (productWishlist == null)
                {
                    return BadRequest("productWishlist data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var model=new ProductWishlist()
                {
                    UserId = productWishlist.UserId,
                    ProductId = productWishlist.ProductId,
                    AddedDate=DateTime.Now,
                };

                bool result = await _dbproductWishListRepository.CreateOrRemoveWishList(model);
                if(result == true)
                {
                    _response.Result = "Add";
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else if(result == false)
                {
                    _response.Result = "Remove";
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    return BadRequest("You data is wrong");
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
        [HttpGet("{Userid:int}/{ProductId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCompanyById(int UserId,int ProductId)
        {
            try
            {
                if (UserId <= 0 || ProductId <= 0)
                {
                    ModelState.AddModelError("Id", "Please Provide Valid Id");
                    return BadRequest(ModelState);
                }
                bool result = await _dbproductWishListRepository.WishListExit(UserId,ProductId);
                if (result == true)
                {
                    _response.Result = "Exit";
                }
                else
                {
                    _response.Result = "NotExit";
                }
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
