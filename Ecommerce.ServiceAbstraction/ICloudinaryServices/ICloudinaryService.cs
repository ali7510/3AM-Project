using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.ICloudinaryServices
{
    public interface ICloudinaryService
    {
        public Task<string> UploadImageAsync(IFormFile file);

        public string CustomImgUrl(string url, int? width, int? height);
    }
}
