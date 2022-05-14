using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IUploadFileService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("UploadImage")]
        [Authorize]
        public JsonResultModel UploadImage()
        {
            try
            {
                var image = _uploadFileService.UploadImage(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
                return JsonResponse.Success(image);
            }
            catch(Exception ex)
            {
                return JsonResponse.ServerError();
            }

        }
    }
}
