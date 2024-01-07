using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EcommerceApi
{
    public class ImageUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<List<ImgPathList>> CreateImage(params IFormFile[] images)
        {
            var paths = new List<ImgPathList>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    string imageUrl = await UploadImage(image);
                    paths.Add(new ImgPathList { Id = paths.Count + 1, Path = imageUrl });
                }
            }

            return paths;
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var imageUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{uniqueFileName}";

            return imageUrl;
        }

        public class ImgPathList
        {
            public int Id { get; set; }
            public string Path { get; set; } = "";
        }
    }
}
