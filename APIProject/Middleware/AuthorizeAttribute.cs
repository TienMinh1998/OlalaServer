using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using APIProject.Domain.Models;
using System;
using APIProject.Service.Utils;
using APIProject.Service.Interfaces;
using System.Collections.Generic;

namespace APIProject.Middleware
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {

        public AuthorizeAttribute() : base(typeof(AuthorizeAction))
        {
            
        }
    }


}
