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
        public async Task<IActionResult> get(string? sort , int? categoryId , int PageSize, int pageNumber)
        {
            
                var product = await _unit.productRepository.GetAllAsync(sort , categoryId , PageSize , pageNumber);
                if (product is null)
                {
                    return BadRequest(new ApiResponse(400));
                }    
                return Ok(product);
           
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
        [HttpPost("add-Product")]
        public async Task<IActionResult> Add(AddProductDTO productDTO)
        {
            
            if (ModelState.IsValid & productDTO is not null)
            {
                var result = await _unit.productRepository.AddAsync(productDTO);
                return Ok(new ApiResponse(200 , "Product Created Successfuly"));
            }
            return BadRequest(new ApiResponse(400));

        }
        [HttpPut("update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            if (ModelState.IsValid & updateProductDTO is not null)
            {
                await _unit.productRepository.UpdateAsync(updateProductDTO);
                return Ok(new ApiResponse(200 , "Product Updated Successfuly"));
            }
            return BadRequest(new ApiResponse(400 ));
        }
        [HttpDelete("delete-Product/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var product =await _unit.productRepository.GetByIdAsync(Id, p => p.Photos, c => c.Category);
            if (product is not null)
            {
                await _unit.productRepository.DeleteAsync(product);
                return Ok(new ApiResponse(200, "Product Deleted Successfully"));
            }
            return BadRequest(new ApiResponse(400));
        }

    }
}
