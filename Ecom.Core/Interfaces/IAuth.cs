using Ecom.Core.DTO.IdentityDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task SendEmail(string email, string Token, string component, string subject, string message);
        Task<string> Login(LoginDTO loginDTO);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(resetPasswordDTO resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccount);
    }
}
