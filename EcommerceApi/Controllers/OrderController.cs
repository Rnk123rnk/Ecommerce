using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/OrderAPI")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _dborderRepository;
        private readonly IOrderShippingRepository _dborderShippingRepository;
        private readonly IOrderDetailRepository _dborderDetailRepository;
        private readonly ICartRepository _dbcartRepository;

        protected ApiResponse _response;

        public OrderController(IOrderRepository dborderRepository,IOrderShippingRepository orderShippingRepository, IOrderDetailRepository dborderDetailRepository, ICartRepository dbcartRepository)
        {
            _dborderRepository = dborderRepository;
            _dborderShippingRepository = orderShippingRepository;
            this._response = new ApiResponse();
            _dborderDetailRepository = dborderDetailRepository;
            _dbcartRepository = dbcartRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> Createorder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null)
                {
                    return BadRequest("Order data is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var Ordermodel = new Order()
                {
                    UserId = orderDTO.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = orderDTO.TotalAmount,
                    IsShipped = false,
                    PaymentMode = orderDTO.PaymentMode,
                    ProductStatus = "Pending",
                    IsCancel=false,
                    RazorpayPaymentId=orderDTO.RazorpayPaymentId,
                };
                if(orderDTO.RazorpayPaymentId == "0")
                {
                    Ordermodel.IsPaymentReceived = false;
                }
                else
                {
                    Ordermodel.IsPaymentReceived = true;
                }

                int OrderIdSet = await _dborderRepository.CreateOrder(Ordermodel);

                var Shippingmodel = new OrderShipping()
                {
                    FullName = orderDTO.Shipping.FullName,
                    Address = orderDTO.Shipping.Address,
                    City = orderDTO.Shipping.City,
                    State = orderDTO.Shipping.State,
                    MobileNumber = orderDTO.Shipping.MobileNumber,
                    PinCode = orderDTO.Shipping.PinCode,
                    OrderId = OrderIdSet,
                };
                await _dborderShippingRepository.CreateOrderShipping(Shippingmodel);

                if (orderDTO.OrderDetails != null && orderDTO.OrderDetails.Any())
                {
                    foreach (var detail in orderDTO.OrderDetails)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = OrderIdSet,
                            ProductId = detail.productId,
                            ProductName = detail.productName,
                            Quantity = detail.quantity,
                            UnitPrice = detail.productPrice,
                            DiscountPrice = detail.productPriceDiscount,
                        };
                        await _dborderDetailRepository.CreateOrderDetail(orderDetail);
                    }
                }
                await _dbcartRepository.DeleteCartByUserId(orderDTO.UserId);
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
    }
}
