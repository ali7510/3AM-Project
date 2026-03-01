using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Ecommerce.ServiceAbstraction.ICloudinaryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.CloudinaryServices
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var settings = config.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public string CustomImgUrl(string url, int? width, int? height)
        {
            if (string.IsNullOrEmpty(url) || !url.Contains("/upload/")) return url;

            var transformation = "upload/";
            if (width.HasValue) transformation += $"w_{width},";
            if (height.HasValue) transformation += $"h_{height},";

            transformation += "c_fill/";
            return url.Replace("upload/", transformation); 
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.");

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "Products", 
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary Upload Error: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }
    }
}
