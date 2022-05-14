using APIProject.Common.Models.Order;
using APIProject.Common.Models.Overview;
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
    public class StatisticService : IStatisticService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public StatisticService(IOrderRepository orderRepository, IMapper mapper, IHub sentryHub, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }


        public async Task<JsonResultModel> GetListSales(int page, int limit, string orderCode, string customerName, string startDate, string endDate)
        {
            try
            {
                var model = await _orderRepository.GetListSales(page, limit, orderCode, customerName, startDate, endDate);
                DataPagedListModel data = new DataPagedListModel
                {
                    Data = model,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = model.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetOverview()
        {
            try
            {
                var model = new OverviewModel
                {
                    TotalCustomer = await _customerRepository.CountCustomer(),
                    TotalOrder = await _orderRepository.CountOrder(),
                    TotalProduct = await _productRepository.CountProduct()
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetTotalSale(string orderCode, string customerName, string startDate, string endDate)
        {
            try
            {
                var model = await _orderRepository.GetTotalSale(orderCode, customerName, startDate, endDate);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
