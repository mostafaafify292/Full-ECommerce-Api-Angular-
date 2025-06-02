using AutoMapper;
using Ecom.API.Error;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ICustomerBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(ICustomerBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket customerBasket)
        {
            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
            return Ok(createdOrUpdatedBasket);
        }
        [HttpDelete("delete-basket/{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var result = await _basketRepository.DeleteAsync(id);
            return result ? Ok(new ApiResponse(200,"item deleted")) : BadRequest(new ApiResponse(400,"can't delete this basket"));
        }
    }
}
