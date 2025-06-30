using Ecom.API.Error;
using Ecom.Core.DTO;
using Ecom.Core.ServicesContract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]

        public async Task<IActionResult> CreateOrder(orderDTO orderDTO)
        {
            if (orderDTO is null)
            {
                return BadRequest(new ApiResponse(400, "Invalid order data"));
            }
            var buyerEmail = User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            }
            var order = await _orderService.CreateOrdersAsync(orderDTO, buyerEmail);
            if (order is null)
            {
                return BadRequest(new ApiResponse(400, "Failed to create order"));
            }
            return Ok(order);
        }
        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> getOrders()
        {
            var buyerEmail = User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            }
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
            if (orders is null || !orders.Any())
            {
                return NotFound(new ApiResponse(404, "No orders found for this user"));
            }
            return Ok(orders);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> getOrderById(int id)
        {
            var buyerEmail = User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            }
            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail);
            if (order is null)
            {
                return NotFound(new ApiResponse(404, "Order not found"));
            }
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            if (deliveryMethods is null || !deliveryMethods.Any())
            {
                return NotFound(new ApiResponse(404, "No delivery methods found"));
            }
            return Ok(deliveryMethods);
        }
    }
}
