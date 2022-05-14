/*-----------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 29/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Status   : Đang làm
 * Content  : Model cho phần Tin tức
 * ----------------------------------*/

using System;
namespace APIProject.Common.Models.News
{
   public class NewsWebModel
    {
        public int ID { get; set; }
        public string  Title { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public int TypeNews  { get; set; }
    }
}
