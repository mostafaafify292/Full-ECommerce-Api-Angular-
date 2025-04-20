using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IImageMangementService
    {
        public Task<List<string>> AddImageAsync(IFormFileCollection files, string folderName);
        public void RemoveImageAsync(string folderName);
        

       
    }
}
