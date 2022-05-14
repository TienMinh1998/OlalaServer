using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class OrderComplainImageRepository : BaseRepository<OrderComplainImage>, IOrderComplainImageRepository
    {
        public OrderComplainImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
