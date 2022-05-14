using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class StorageExportDetailRepository : BaseRepository<StorageExportDetail>, IStorageExportDetailRepository
    {
        public StorageExportDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
