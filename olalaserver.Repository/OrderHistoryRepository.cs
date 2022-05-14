using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class OrderHistoryRepository : BaseRepository<OrderHistory>, IOrderHistoryRepository
    {
        public OrderHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
