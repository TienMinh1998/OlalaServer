using APIProject.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IHub _sentryHub;

        public UploadFileService(IHub sentryHub)
        {
            _sentryHub = sentryHub;
        }

        public List<string> UploadImages(string FileName, HttpContext context, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                List<string> listImage = new List<string>();
                var httpRequest = context.Request;
                var postedFile = httpRequest.Form.Files.GetFiles(FileName);
                if (postedFile != null && postedFile.Count > 0)
                {
                    var folderName = Path.Combine("UploadFile", "Images");
                    var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, folderName);

                    var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                    foreach (var file in postedFile)
                    {
                        string name = DateTime.Now.ToString("ssddMMyyyy") + file.FileName;
                        var fullPath = Path.Combine(pathToSave, name);
                        var url = host + "/UploadFile/Images/" + name;
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        //Thread ThreadNotificationToCustomer = new Thread(() =>
                        //{
                        //    using (var stream = new FileStream(fullPath, FileMode.Create))
                        //    {
                        //        file.CopyTo(stream);
                        //    }
                        //});
                        //ThreadNotificationToCustomer.Start();
                        //ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        //{
                        //    using (var stream = new FileStream(fullPath, FileMode.Create))
                        //    {
                        //        file.CopyTo(stream);
                        //    }
                        //}));
                        listImage.Add(url);
                    }
                    return listImage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return null;
            }

        }
        public string UploadImage(string FileName, HttpContext context, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                var httpRequest = context.Request;
                var postedFile = httpRequest.Form.Files.GetFile(FileName);
                if (postedFile != null && postedFile.Length > 0)
                {
                    var folderName = Path.Combine("UploadFile", "Images");
                    var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, folderName);

                    var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

                    string name = DateTime.Now.ToString("ssddMMyyyy") + postedFile.FileName;
                    var fullPath = Path.Combine(pathToSave, name);
                    var url = host + "/UploadFile/Images/" + name;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    //ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                    //{
                    //    using (var stream = new FileStream(fullPath, FileMode.Create))
                    //    {
                    //        postedFile.CopyTo(stream);
                    //    }
                    //}));
                    //Thread thread = new Thread(() =>
                    //{
                    //    using (var stream = new FileStream(fullPath, FileMode.Create))
                    //    {
                    //        postedFile.CopyTo(stream);
                    //    }
                    //});
                    //thread.Start();

                    return url;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                throw ex;
            }

        }
    }
}
