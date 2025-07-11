﻿using Ecom.Core.DTO;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.Entites.Identity;
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
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(resetPasswordDTO resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccount);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> getUserAddress(string email);
    }
}
