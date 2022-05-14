using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Address
{
    public class ProvinceModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class DistrictModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProvinceID { get; set; }
        public string Type { get; set; }
    }
    public class WardModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DistrictID { get; set; }
        public string Type { get; set; }
    }
}
