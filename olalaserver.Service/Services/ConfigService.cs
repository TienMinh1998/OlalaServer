using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Config;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class ConfigService : IConfigService
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public ConfigService(IMapper mapper, IHub sentryHub, IContactRepository contactRepository, ICustomerTypeRepository customerTypeRepository)
        {
            _mapper = mapper;
            _sentryHub = sentryHub;
            _contactRepository = contactRepository;
            _customerTypeRepository = customerTypeRepository;
        }

        public async Task<JsonResultModel> GetListContact()
        {
            try
            {
                var model = await _contactRepository.GetAllAsync();
                return JsonResponse.Success(model);
            }catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListCustomerType()
        {
            try
            {
                var model = await _customerTypeRepository.GetAllAsync(x => !x.ID.Equals(SystemParam.CUSTOMER_TYPE_NORMAL));
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateContact(UpdateContactModel input)
        {
            try
            {
                var model = await _contactRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                model.Value = input.Value;
                await _contactRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateCustomerType(UpdateCustomerTypeModel input)
        {
            try
            {
                var model = await _customerTypeRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                model.BonusPointPerKg = input.BonusPointPerKg;
                await _customerTypeRepository.UpdateAsync(model);
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
