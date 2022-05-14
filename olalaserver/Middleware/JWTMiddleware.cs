
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using APIProject.Service.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Interface;
using Microsoft.Extensions.Configuration;
using APIProject.Service.Utils;

namespace APIProject.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration Configuration;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context, ICustomerService _customerService, IUserService _userService, IPermissionService _permissionService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            await AttachUserToContext(context, token, _customerService, _userService, _permissionService);
            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token, ICustomerService _customerService, IUserService _userService, IPermissionService _permissionService)
        {
            try
            {
                if (token == null)
                {
                    context.Items["AuthCode"] = SystemParam.TOKEN_INVALID;
                }
                else
                {
                    PayloadModel payload = CheckToken(token);
                    if (payload.TokenType.Equals(SystemParam.TOKEN_TYPE_CUSTOMER))
                    {
                        var cus = await _customerService.GetFirstOrDefaultAsync(x => x.ID.Equals(payload.ID));
                        if (cus == null)
                        {
                            context.Items["AuthCode"] = SystemParam.TOKEN_INVALID;
                        }
                        else if (cus.Token != token)
                        {
                            context.Items["AuthCode"] = SystemParam.TOKEN_ERROR;
                        }
                        else
                        {
                            context.Items["Payload"] = cus;
                        }
                    }
                    else
                    {
                        var user = await _userService.GetFirstOrDefaultAsync(x => x.ID.Equals(payload.ID));
                        if (user == null)
                        {
                            context.Items["AuthCode"] = SystemParam.TOKEN_INVALID;
                        }
                        else if (user.Token != token)
                        {
                            context.Items["AuthCode"] = SystemParam.TOKEN_ERROR;
                        }
                        else
                        {
                            context.Items["Payload"] = user;
                            var permission = await _permissionService.GetListRolePermission(user.RoleID);
                            context.Items["Permission"] = permission;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public PayloadModel CheckToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Configuration["AppSettings:Secret"];
                var key = Encoding.ASCII.GetBytes(secretKey);
                // Giải mã Key Truyền lên
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return new PayloadModel
                {
                    ID = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
                    TokenType = jwtToken.Claims.First(x => x.Type == "type").Value
                };
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new PayloadModel
                {
                    ID = 0,
                    TokenType = SystemParam.TOKEN_TYPE_CUSTOMER
                };
            }
        }
    }
}
