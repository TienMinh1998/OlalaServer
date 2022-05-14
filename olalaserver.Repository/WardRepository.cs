using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class WardRepository : BaseRepository<Ward>, IWardRepository
    {
        public WardRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
