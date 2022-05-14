using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.MemberPointHistory
{
    public class MemberPointHistoryModel
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
        public double? TotalWeight { get; set; }
        public long Balance { get; set; }
        public long Point { get; set; }
        public int Type { get; set; }
        public int TypeAdd { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
