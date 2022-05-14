using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using Quartz;
using APIProject.Service.Interfaces;
using APIProject.Service.Interface;
using APIProject.Service.Utils;
using Microsoft.Extensions.Configuration;

namespace APIProject.CronJob
{
    public class Jobclass : IJob
    {
        private readonly IOrderService _OrderService;
        private readonly IConfiguration _Configuration;

        public Jobclass(IConfiguration configuration, IOrderService orderService)
        {
            _Configuration = configuration;
            _OrderService = orderService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CheckTaskService()
        {
            var completeTime = SystemParam.COMPLETE_ORDER_TIME;
            try
            {
                completeTime = int.Parse(_Configuration["CompleteOrderTime"]);
            }
            catch
            {
                completeTime = SystemParam.COMPLETE_ORDER_TIME;
            }
            await _OrderService.CompleteOrderProcedure(completeTime);



        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.WhenAll(CheckTaskService());
            }
            catch
            {
                return;
            }
        }
    }
}