using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class ProductItemRepository : BaseRepository<ProductItem>, IProductItemRepository
    {
        public ProductItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


    }
}
