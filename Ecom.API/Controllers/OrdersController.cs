using Ecom.API.Error;
using Ecom.Core.DTO;
using Ecom.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var buyerEmail = User?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value;
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
    }
}
