using Ecom.Core.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.ServicesContract
{
    public interface IGenerateToken
    {
        public Task<string> GetAndCreateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
