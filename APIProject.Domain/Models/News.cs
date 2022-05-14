using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class News : BaseModel
    {
        public string Title { get; set; }   // Tiêu đề của tin 
        public int TypeNews { get; set; }    // Kiểu tin tức
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public int Status { get; set; } = SystemParam.ACTIVE;
        public int Type { get; set; } // kiểu đăng bài / lưu nháp

    }
}
