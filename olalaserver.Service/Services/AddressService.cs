using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Address;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IProvinceRepository _ProvinceRepository;
        private readonly IDistrictRepository _DistrictRepository;
        private readonly IWardRepository _WardRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public AddressService(IMapper mapper, IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository, IHub sentryHub)
        {
            _mapper = mapper;
            _ProvinceRepository = provinceRepository;
            _DistrictRepository = districtRepository;
            _WardRepository = wardRepository;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetProvinces()
        {
            try
            {
                var model = await _ProvinceRepository.GetAllAsync(null,source => source.OrderBy(x => x.Name));
                var provinces = _mapper.Map<List<ProvinceModel>>(model);
                return JsonResponse.Success(provinces);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetDistricts(int ProvinceID)
        {
            try
            {
                var model = await _DistrictRepository.GetAllAsync(x => x.ProvinceCode.Equals(ProvinceID),source => source.OrderBy(x => x.Name));
                var districts = _mapper.Map<List<DistrictModel>>(model);
                return JsonResponse.Success(districts);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> GetWards(int DistrictID)
        {
            try
            {
                var model = await _WardRepository.GetAllAsync(x => x.District_id.Equals(DistrictID), source => source.OrderBy(x => x.Name));
                var wards = _mapper.Map<List<WardModel>>(model);
                return JsonResponse.Success(wards);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
