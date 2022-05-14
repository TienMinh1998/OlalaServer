using APIProject.Common.Models.Role;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIProject.Service.Utils;

namespace APIProject.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<RoleModel>> GetListRole()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var role = (from r in DbContext.Roles
                                where r.IsActive.Equals(SystemParam.ACTIVE)
                                orderby r.ID descending
                                select new
                                {
                                    ID = r.ID,
                                    Name = r.Name,
                                    RolePermissions = DbContext.RolePermissions.Include(x => x.Permission).Where(x => x.Role.Equals(r.ID))
                                }).AsEnumerable().Select(x => new RoleModel
                                {
                                    ID = x.ID,
                                    Name = x.Name,
                                    ListPermission = x.RolePermissions.Select(p => new PermissionModel
                                    {
                                        ID = p.Permission.ID,
                                        Name = p.Permission.Name
                                    }).ToList()
                                }).ToList();
                    return role;
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleModel> GetRoleDetail(int ID)
        {
            try
            {
                var role = await (from r in DbContext.Roles
                            where r.ID.Equals(ID) && r.IsActive.Equals(SystemParam.ACTIVE)
                            select new RoleModel
                            {
                                ID = r.ID,
                                Name = r.Name,
                                ListPermission = DbContext.RolePermissions.Include(x => x.Permission).Where(x => x.Role.Equals(r.ID)).Select(p => new PermissionModel
                                {
                                    ID = p.Permission.ID,
                                    Name = p.Permission.Name
                                }).ToList()
                            }).FirstOrDefaultAsync();
                return role;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
