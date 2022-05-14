/*------------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 19/1/2022
 * Edit     : Chưa chỉnh Sửa
 * Status   : Đã Xong
 * Content  : Model New, Chưa có ảnh
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.News
{
  public class NewAddInputModel
    {
        public string Title { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int TypeNews { get; set; }
        public bool SentNotification { get; set; }
        public string UrlImage { get; set; }
    }
}
