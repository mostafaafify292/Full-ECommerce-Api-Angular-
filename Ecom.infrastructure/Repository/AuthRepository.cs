using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.Entites.Identity;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ecom.infrastructure.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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


            return "done";
        }
    }
}
