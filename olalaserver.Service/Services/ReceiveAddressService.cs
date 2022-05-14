using APIProject.Common.Models.ReceiveAddress;
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
    public class ReceiveAddressService : BaseService<ReceiveAddress>, IReceiveAddressService
    {
        private readonly IReceiveAddressRepository _ReceiveAddressRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public ReceiveAddressService(IReceiveAddressRepository ReceiveAddressRepository, IMapper mapper, IHub sentryHub) : base(ReceiveAddressRepository)
        {
            _ReceiveAddressRepository = ReceiveAddressRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetReceiveAddresses(int CusID, string Search)
        {
            try
            {
                var model = await _ReceiveAddressRepository.GetReceiveAddresses(CusID, Search);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetReceiveAddressDefault(int CusID)
        {
            try
            {
                var model = await _ReceiveAddressRepository.GetReceiveAddressDefault(CusID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> GetReceiveAddressDetail(int ID)
        {
            try
            {
                var model = await _ReceiveAddressRepository.GetReceiveAddressDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_RECEIVE_ADDRESS_NOT_EXIST, SystemParam.MESSAGE_RECEIVE_ADDRESS_NOT_EXIST);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateReceiveAddress(AddReceiveAddressModel input, int CusID)
        {
            try
            {
                var model = _mapper.Map<ReceiveAddress>(input);
                model.CustomerID = CusID;
                var receiveAddress = await _ReceiveAddressRepository.AddAsync(model);
                if (model.IsDefault.Equals(SystemParam.RECEIVE_ADDRESS_DEFAULT))
                {
                    var addressDefault = await _ReceiveAddressRepository.GetFirstOrDefaultAsync(x => x.CustomerID.Equals(CusID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.IsDefault.Equals(SystemParam.RECEIVE_ADDRESS_DEFAULT) && !x.ID.Equals(receiveAddress.ID));
                    if (addressDefault != null)
                    {
                        addressDefault.IsDefault = SystemParam.RECEIVE_ADDRESS_NOT_DEFAULT;
                        await _ReceiveAddressRepository.UpdateAsync(addressDefault);
                    }

                }
                return JsonResponse.Success(receiveAddress.ID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> UpdateReceiveAddress(UpdateReceiveAddressModel input, int CusID)
        {
            try
            {
                var checkAddress = await _ReceiveAddressRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (checkAddress == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_RECEIVE_ADDRESS_NOT_EXIST, SystemParam.MESSAGE_RECEIVE_ADDRESS_NOT_EXIST);
                }
                var model = _mapper.Map<ReceiveAddress>(input);
                model.CustomerID = CusID;
                var receiveAddress = await _ReceiveAddressRepository.UpdateAsync(model);
                if (model.IsDefault.Equals(SystemParam.RECEIVE_ADDRESS_DEFAULT))
                {
                    var addressDefault = await _ReceiveAddressRepository.GetFirstOrDefaultAsync(x => x.CustomerID.Equals(CusID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.IsDefault.Equals(SystemParam.RECEIVE_ADDRESS_DEFAULT) && !x.ID.Equals(receiveAddress.ID));
                    if (addressDefault != null)
                    {
                        addressDefault.IsDefault = SystemParam.RECEIVE_ADDRESS_NOT_DEFAULT;
                        await _ReceiveAddressRepository.UpdateAsync(addressDefault);
                    }

                }
                return JsonResponse.Success(receiveAddress.ID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteReceiveAddress(int ID)
        {
            try
            {
                var model = await _ReceiveAddressRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_RECEIVE_ADDRESS_NOT_EXIST, SystemParam.MESSAGE_RECEIVE_ADDRESS_NOT_EXIST);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                var address = await _ReceiveAddressRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
