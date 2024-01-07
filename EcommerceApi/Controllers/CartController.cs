using Azure;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models.Dto;
using Ecommerce.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/CartAPI")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductsRepository _dbproductsRepository;
        private readonly ICartRepository _dbcartRepository;
        protected ApiResponse _response;

        public CartController(IProductsRepository dbproductsRepository, ICartRepository dbcartRepository)
        {
            _dbproductsRepository = dbproductsRepository;
            _dbcartRepository = dbcartRepository;
            this._response = new ApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CreateCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                if (cartDTO == null)
                {
                    return BadRequest("CreateCart data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _dbproductsRepository.ProductIdExit(cartDTO.ProductId) == false)
                {
                    return NotFound("Product not found");
                }
                var model = new Cart()
                {
                    ProductId = cartDTO.ProductId,
                    Quantity = cartDTO.Quantity,
                    UserId = cartDTO.UserId,
                    SessionId = cartDTO.SessionId,

                };
                bool result=  await _dbcartRepository.CreateCart(model);
                if (result == false)
                {
                   return BadRequest("You can't add more than the available quantity\"");
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

        [HttpGet("{id:int}/{randomuser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCartById(int id, string randomuser)
        {
            try
            {
                if (id == 0 && randomuser == null)
                {
                    return BadRequest("Userid And RandomUser Both Are Null");
                }
                var cart = await _dbcartRepository.GetCartByIdList(id, randomuser);
                if (cart == null)
                {
                    return NotFound("Product do not exit in cart");
                }
                _response.Result = cart;
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
        public async Task<ActionResult<ApiResponse>> DeleteCart(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool cart = await _dbcartRepository.DeleteCart(id);
                if (!cart)
                {
                    return NotFound("Cart not exists Please check id");
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
