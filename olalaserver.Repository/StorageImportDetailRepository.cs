using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class StorageImportDetailRepository : BaseRepository<StorageImportDetail>, IStorageImportDetailRepository
    {
        public StorageImportDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
