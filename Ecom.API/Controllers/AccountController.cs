using AutoMapper;
using Ecom.API.Error;
using Ecom.Core.DTO;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.Entites.Identity;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                Expires = DateTime.Now.AddDays(1),
                
                IsEssential =true,
                SameSite = SameSiteMode.None,
                Path="/"
            });
            return Ok(new ApiResponse(200, "Login SUCCESS"));
        }

        [HttpPost("active-account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await _unit.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ApiResponse(200 , "Active Successfully")) : BadRequest(new ApiResponse(400));
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

        [HttpPut("update-address")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> updateAddress(ShipAddressDTO shipAddressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = _mapper.Map<Address>(shipAddressDTO);
            var result = await _unit.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("isUserAuth")]
        public async Task<IActionResult> isUserAuth()
        {

            // Force the authentication middleware to run
            var authenticateResult = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return Unauthorized("User is not authenticated");

            return Ok("User is authenticated");
        }

        [HttpGet("get-address-for-user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getAddress()
        {
            var address = await _unit.Auth.getUserAddress(User.FindFirst(ClaimTypes.Email)?.Value);
            if (address is not null)
            {
                var result =  _mapper.Map<ShipAddressDTO>(address);
                return Ok(result);
            }
            return BadRequest(new ApiResponse(404 , "User Address is null"));

        }
    } 
}
