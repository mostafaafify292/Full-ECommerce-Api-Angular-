using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.Entites.Identity;
using Ecom.Core.Interfaces;
using Ecom.Core.ServicesContract;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Identity;

namespace Ecom.infrastructure.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IGenerateToken _token;

        public AuthRepository(UserManager<AppUser> userManager ,
                              SignInManager<AppUser> signInManager ,
                              IEmailService emailService ,
                              IGenerateToken token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _token = token;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this UserName is already registerd";
            }
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this Email is already registerd";
            }
            var user = new AppUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,

            };
            var result = await _userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded is not true) 
            {
                return result.Errors.ToList()[0].Description;
            }
            //send active email 
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token , "active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");
            return "done";
        }

        //Send Email
        public async Task SendEmail(string email , string code , string component , string subject ,string message)
        {
            var result = new EmailDTO(email,
                                     "mostafaafify291@gmail.com",
                                     subject,
                                     EmailStringBody.send(email, code, component, message));
             await _emailService.SendEmail(result);
        }

        //Login
        public async Task<string> Login(LoginDTO loginDTO) 
        {
            if (loginDTO == null)
            {
                return null;
            }
            var findUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (!findUser.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token,"active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");
                return "Please Confirem your E-mail First , we have send activate to your E-mail";
            }
            var result = await _signInManager.CheckPasswordSignInAsync(findUser, loginDTO.Password, false);
            if (result.Succeeded)
            {
                return await _token.GetAndCreateToken(findUser , _userManager);
            }
            return "Please check your email or password , something went wrong";
        }

        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");
        }
    }
}
