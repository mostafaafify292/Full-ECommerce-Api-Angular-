using AutoMapper;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unit , IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            var result = await _unit.Auth.RegisterAsync(registerDTO);

        }
    }
}
