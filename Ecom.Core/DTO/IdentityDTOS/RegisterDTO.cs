using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO.IdentityDTOS
{
    public record LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public record RegisterDTO : LoginDTO
    {
        [Required]
        public string UserName { get; set; }

    }
    public record resetPasswordDTO : LoginDTO
    {
        public string Token { get; set; }
    }
    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
