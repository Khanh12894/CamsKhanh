using Microsoft.AspNetCore.Http;

namespace Cams.WebApi.Models.Upload
{
    public class UploadModel
    {
        public IFormFile File { get; set; }
    }
}
