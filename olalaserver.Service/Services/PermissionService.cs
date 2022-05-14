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

namespace APIProject.Service.Services
{
    public class PermissionService : BaseService<Permission>, IPermissionService
    {
        private readonly IPermissionRepository _PermissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public PermissionService(IPermissionRepository PermissionRepository, IMapper mapper, IHub sentryHub, IRolePermissionRepository rolePermissionRepository) : base(PermissionRepository)
        {
            _PermissionRepository = PermissionRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<JsonResultModel> GetListPermission()
        {
            try
            {
                var model = await _PermissionRepository.GetAllAsync();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<List<int>> GetListRolePermission(int RoleID)
        {
            try
            {
                return await _rolePermissionRepository.GetListRolePermission(RoleID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return null;
            }
        }
    }
}
