using Microsoft.AspNetCore.Http;

namespace mvc.Models.DTO
{
    public class FileUpload 
    {
        public IFormFile FormFile {get;set;}
    }
}