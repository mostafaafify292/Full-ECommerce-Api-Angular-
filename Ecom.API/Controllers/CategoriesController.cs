using AutoMapper;
using Ecom.API.Error;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork  unit , IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            
                var category = await _unit.Repository<Category>().GetAllAsync();
                if (category is null)
                {
                    return BadRequest( new ApiResponse(400));
                
                }
                return Ok(category);
        
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            
                var category = await _unit.Repository<Category>().GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest(new ApiResponse(400 , $"not Found Category Id = {id}"));
                }
                return Ok(category);


        }

        [HttpPost("add-category")]
        public async Task<IActionResult> addCategory(CategoryDTO categoryDTO)
        {
           
                var category = _mapper.Map< CategoryDTO , Category>(categoryDTO);
              
                await _unit.Repository<Category>().AddAsync(category);
                await _unit.CompleteAsync();
                return Ok(new ApiResponse(200, "item has been Added"));

            
           
        }

        [HttpPut ("update-category")]
        public async Task<IActionResult> updateCategory(UpdateCategoryDTO categoryDTO)
        {
           
                var category = _mapper.Map<Category>(categoryDTO);

                _unit.Repository<Category>().UpdateAsync(category);
                await _unit.CompleteAsync();
                return Ok(new ApiResponse(200, "item has been Updateed"));

         
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> deleteCategory(int id)
        {
           
                await _unit.Repository<Category>().DeleteAsync(id);
                await _unit.CompleteAsync();
                return Ok(new ApiResponse(200, "item has been Deleted"));
  
        }
    }
}
