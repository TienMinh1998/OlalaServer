using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Order
{
    public class ComplainOrderModel
    {
        public int ID { get; set; }
        public string NoteComplain { get; set; }
        public List<string> ListImageUrl { get; set; }
    }
}
