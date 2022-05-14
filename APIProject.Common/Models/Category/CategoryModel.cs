using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Category
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime createdate { get; set; }
    }
    public class CreateCategoryModel
    {
        public string Name { get; set; }
    }
    public class UpdateCategoryModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
