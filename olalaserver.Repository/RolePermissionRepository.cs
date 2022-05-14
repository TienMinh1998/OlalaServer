using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<int>> GetListRolePermission(int RoleID)
        {
            try
            {
                return await DbContext.RolePermissions.Where(x => x.RoleID.Equals(RoleID)).Select(x => x.PermissionID).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
