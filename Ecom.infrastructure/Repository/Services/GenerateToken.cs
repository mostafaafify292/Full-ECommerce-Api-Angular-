using Ecom.Core.Entites.Identity;
using Ecom.Core.ServicesContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository.Services
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration _configuration;

        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GetAndCreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            //Private Claims
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var userRole = await userManager.GetRolesAsync(user);
            foreach (var Role in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));
            var token = new JwtSecurityToken(
                //audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"] ?? "0")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
