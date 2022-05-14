using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Storage;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public StorageService(IStorageRepository storageRepository, IMapper mapper, IHub sentryHub)
        {
            _storageRepository = storageRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetListStorage()
        {
            try
            {
                var model = await _storageRepository.GetAllAsync();
                return JsonResponse.Success(model);
            }catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetStorageDetail(int ID)
        {
            try
            {
                var model = await _storageRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                if(model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_NOT_FOUND, SystemParam.MESSAGE_STORAGE_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateStorage(StorageModel input)
        {
            try
            {
                var model = await _storageRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_NOT_FOUND, SystemParam.MESSAGE_STORAGE_NOT_FOUND);
                }
                var storage = _mapper.Map<Storage>(input);
                await _storageRepository.UpdateAsync(storage);
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
