using APIProject.Service.Models;
using APIProject.Service.Models.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IConfigService
    {
        Task<JsonResultModel> GetListCustomerType();
        Task<JsonResultModel> GetListContact();
        Task<JsonResultModel> UpdateContact(UpdateContactModel input);
        Task<JsonResultModel> UpdateCustomerType(UpdateCustomerTypeModel input);
    }
    
}
