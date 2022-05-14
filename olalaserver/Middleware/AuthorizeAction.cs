using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Middleware
{
    public class AuthorizeAction : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authCode = (int?)context.HttpContext.Items["AuthCode"];
            if (authCode == SystemParam.TOKEN_INVALID)
            {
                context.Result = new JsonResult(JsonResponse.Error(SystemParam.TOKEN_INVALID, SystemParam.MESSAGE_TOKEN_INVALID))
                { };
            }
            else if (authCode == SystemParam.TOKEN_ERROR)
            {
                context.Result = new JsonResult(JsonResponse.Error(SystemParam.TOKEN_ERROR, SystemParam.MESSAGE_TOKEN_ERROR))
                { };
            }
        }
    }
}
