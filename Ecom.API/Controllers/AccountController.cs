using AutoMapper;
using Ecom.API.Error;
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
            if (result != "done")
            {
                return BadRequest(new ApiResponse(400, result));
            }
            return Ok(new ApiResponse(200, result));


        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {

            var result = await _unit.Auth.LoginAsync(loginDTO);

            if (result.StartsWith("Please"))
            {
                return BadRequest(new ApiResponse(400, result));
            }

            Response.Cookies.Append("token", result, new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential =true,
                SameSite = SameSiteMode.Strict
            });
            return Ok(new ApiResponse(200));
        }
    }
}
