using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IUploadFileService
    {
        List<string> UploadImages(string FileName,HttpContext context, IWebHostEnvironment webHostEnvironment);
        string UploadImage(string FileName,HttpContext context, IWebHostEnvironment webHostEnvironment);
    }
}
