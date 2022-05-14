using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Statistic
{
    public class StatisticModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public long SumPrice { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
