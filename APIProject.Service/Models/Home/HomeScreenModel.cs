using APIProject.Common.Models.News;
using APIProject.Common.Models.Product;
using APIProject.Service.Models.News;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Home
{
   public class HomeScreenModel
     {
        public IPagedList<ProductAppModel> ListSaleProduct { get; set; }           // Sản phẩm khuyến mại 
        public IPagedList<ProductAppModel> ListHotProduct { get; set; }            // sản phẩm hot 
        public IPagedList<ProductAppModel> ListProductTrend { get; set; }         // sản phẩm bán chạy
        public IPagedList<ProductAppModel> ListProduct { get; set; }              // sản phẩm
        public List<NewsModel> ListBanner { get; set; }                           // danh sách banner

    }

     public class CustomerLogin
    {
        public string CustomerName { get; set; }
        public int Point { get; set; }
    }
}
