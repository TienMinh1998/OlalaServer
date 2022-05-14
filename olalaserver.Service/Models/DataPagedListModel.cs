using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models
{
    public class DataPagedListModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalItemCount { get; set; }
        public object Data { get; set; }
    }
}
