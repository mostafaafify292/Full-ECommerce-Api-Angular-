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

        [HttpPost("active-account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await _unit.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ApiResponse(200)) : BadRequest(new ApiResponse(400));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await _unit.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ApiResponse(200)) : BadRequest(new ApiResponse(400));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> reset(resetPasswordDTO passwordDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _unit.Auth.ResetPassword(passwordDTO);
                if (result != "Password change success")
                {
                    return BadRequest(new ApiResponse(400, result));
                }
                return Ok(new ApiResponse(200, result));
            }
      
            return BadRequest(new ApiResponse(400, "Model is not Valid"));
            
        }
    } 
}
