using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.News
{
  public  class UpdateNewsModel 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int TypeNews { get; set; }
        public string URLImage { get; set; }
        public bool SentNotification { get; set; }
    }

    public class UpdateNewsInputModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int TypeNews { get; set; }
        public string UrlImage { get; set; }

    }
}
