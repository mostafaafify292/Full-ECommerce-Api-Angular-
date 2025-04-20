using AutoMapper;
using Ecom.API.Error;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unit, IMapper mapper )
        {
            _unit = unit;
            _mapper = mapper;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            
                var product = await _unit.Repository<Product>().GetAllAsync(x => x.Category, x => x.Photos);
                if (product is null)
                {
                    return BadRequest(new ApiResponse(400));
                }
                 var productDto = _mapper.Map<List<ProductDTO>>(product);
                return Ok(productDto);
           
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id )
        {
            var product = await _unit.Repository<Product>().GetByIdAsync(id , x=>x.Category , x=>x.Photos);
            if (product is null)
            {
                return BadRequest(new ApiResponse(400));
            }
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }
        //[HttpPost("add-Product")]
        //public async Task<IActionResult> Add(AddProductDTO productDTO)
        //{
            
        //    _unit
        //}
    }
}
