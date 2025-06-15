using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO.IdentityDTOS;

namespace Ecom.Core.ServicesContract
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
