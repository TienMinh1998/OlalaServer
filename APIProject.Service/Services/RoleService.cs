using APIProject.Common.Models.Role;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository _RoleRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IRolePermissionRepository _RolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, IHub sentryHub, IRolePermissionRepository rolePermissionRepository, IUserRepository userRepository) : base(roleRepository)
        {
            _RoleRepository = roleRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _RolePermissionRepository = rolePermissionRepository;
            _UserRepository = userRepository;
        }


        public async Task<JsonResultModel> GetListRole()
        {
            try
            {
                var model = await _RoleRepository.GetListRole();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetRoleDetail(int ID)
        {
            try
            {
                var model = await _RoleRepository.GetRoleDetail(ID);
                if(model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_ROLE_NOT_FOUND, SystemParam.MESSAGE_ROLE_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> DeleteRole(int ID)
        {
            try
            {
                var model = await _RoleRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_ROLE_NOT_FOUND, SystemParam.MESSAGE_ROLE_NOT_FOUND);
                }
                var checkUser = await _UserRepository.GetFirstOrDefaultAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.RoleID.Equals(ID));
                if(checkUser != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_ROLE_USER_STILL_EXIST, SystemParam.MESSAGE_ROLE_USER_STILL_EXIST);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _RoleRepository.UpdateAsync(model);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> CreateRole(CreateRoleModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var checkRole = await _RoleRepository.GetFirstOrDefaultAsync(x => x.Name.Equals(input.Name) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (checkRole != null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ROLE_NAME_ALREADY_EXIST, SystemParam.MESSAGE_ROLE_NAME_ALREADY_EXIST);
                    }
                    var role = new Role
                    {
                        Name = input.Name
                    };
                    await _RoleRepository.AddAsync(role);
                    foreach (var item in input.ListPermissionID)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleID = role.ID,
                            PermissionID = item
                        };
                        await _RolePermissionRepository.AddAsync(rolePermission);
                    }
                    scope.Complete();
                    return JsonResponse.Success("");
                }

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> UpdateRole(UpdateRoleModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var checkRole = await _RoleRepository.GetFirstOrDefaultAsync(x => x.Name.Equals(input.Name) && !x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (checkRole != null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ROLE_NAME_ALREADY_EXIST, SystemParam.MESSAGE_ROLE_NAME_ALREADY_EXIST);
                    }
                    var role = await _RoleRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                    if(role == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ROLE_NOT_FOUND, SystemParam.MESSAGE_ROLE_NOT_FOUND);
                    }
                    role.Name = input.Name;
                    await _RoleRepository.UpdateAsync(role);
                    var listRolePermission = await _RolePermissionRepository.GetAllAsync(x => x.RoleID.Equals(role.ID));
                    foreach(var item in listRolePermission)
                    {
                        await _RolePermissionRepository.DeleteAsync(item);
                    }
                    foreach (var item in input.ListPermissionID)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleID = role.ID,
                            PermissionID = item
                        };
                        await _RolePermissionRepository.AddAsync(rolePermission);
                    }
                    scope.Complete();
                    return JsonResponse.Success("");
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
