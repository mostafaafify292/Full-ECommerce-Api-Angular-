using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository.Services
{
    public class ImageMangementService : IImageMangementService
    {
        private readonly IFileProvider _fileProvider;

        public ImageMangementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string folderName) {
            var saveImageSrc = new List<string>();

            if (files.Count == 0)
                return saveImageSrc;

            folderName = folderName.Trim(); 
            var imageDirectory = Path.Combine("wwwroot", "Images", folderName);

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            foreach (var item in files)
            {
                var imageName = Path.GetFileName(item.FileName); 
                var imageSrc = $"/Images/{folderName}/{imageName}";
                var fullPath = Path.Combine(imageDirectory, imageName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }

                saveImageSrc.Add(imageSrc);
            }

            return saveImageSrc;
        }
            //{
            //    //C:\Users\mostafa.afifi\Desktop\Ecom Full Proj\Ecom\Ecom.API\wwwroot\Files\
            //    //Get FolderPath 
            //    var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            //    //Get FilePath
            //    var fileName= $"{Guid.NewGuid()}{ files.FileName }";
            //    //Get FullPath
            //    var FullPath = Path.Combine(FolderPath, fileName);
            //    //SaveFile As Stream
            //    var fileStream = new FileStream(FullPath, FileMode.Create);
            //    files.CopyTo(fileStream);
            //    return fileName;

        public void RemoveImageAsync(string folderName )
        {
            var info = _fileProvider.GetFileInfo(folderName);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}
