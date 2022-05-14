

/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 04/01/2021
 * Edit     : Đã hoàn thành
 * Content  : Customer Service
 * ----------------------------------*/

using AutoMapper;
using APIProject.Service.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using APIProject.Service.Interface;
using AutoMapper.Configuration;
using APIProject.Service.Utils;
using APIProject.Domain.Models;
using APIProject.Service.Models.Authentication;
using PagedList.Core;
using Sentry;
using APIProject.Service.Models.Customer;
using APIProject.Service.MailService;
using System.Threading;
using APIProject.Common.Models.Password;
using APIProject.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Service.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IMemberPointHistoryRepository _memberPointHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        private readonly IMail _mail;
        private readonly ISocketService _socketService;
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IHub sentryHub, IMemberPointHistoryRepository memberPointHistoryRepository, IMail iMail, IPushNotificationService pushNotificationService, ISocketService socketService) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _memberPointHistoryRepository = memberPointHistoryRepository;
            _mail = iMail;
            _pushNotificationService = pushNotificationService;
            _socketService = socketService;
        }
        private string GenerateJwtToken(string cusID, string secretKey, int timeout)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("id",cusID),
                new Claim("type",SystemParam.TOKEN_TYPE_CUSTOMER)
                }),
                Expires = DateTime.UtcNow.AddHours(timeout), // THời gian tồn tại của Token :
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // Login App : 
        public async Task<JsonResultModel> Authenticate(LoginModel model, string secretKey, int timeout)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Password))
                    return JsonResponse.Error(SystemParam.ERROR_LOGIN_FIELDS_INVALID, SystemParam.MESSAGE_LOGIN_FIELDS_INVALID);
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone == model.Phone);
                if (cus == null) return JsonResponse.Error(SystemParam.ERROR_LOGIN_FAIL, SystemParam.MESSAGE_LOGIN_FAIL);
                if (!Util.CheckPass(model.Password, cus.Password)) return JsonResponse.Error(SystemParam.ERROR_LOGIN_FAIL, SystemParam.MESSAGE_LOGIN_FAIL);
                var token = GenerateJwtToken(cus.ID.ToString(), secretKey, timeout);
                cus.Token = token;
                cus.DeviceID = model.DeviceID;
                // update customer to database
                await _customerRepository.UpdateAsync(cus);
                return JsonResponse.Success(cus);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> Register(RegisterModel model, string secretKey, int timeout)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.Password) || model.CustomerTypeID == 0)
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_FIELDS_INVALID, SystemParam.MESSAGE_REGISTER_FIELDS_INVALID);
                if (!Util.validPhone(model.Phone))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                if (!Util.ValidateEmail(model.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                var cusPhone = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cusPhone != null)
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_EXIST, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);
                var cusEmail = await _customerRepository.GetFirstOrDefaultAsync(x => x.Email.Equals(model.Email) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cusEmail != null)
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_EXIST, SystemParam.MESSAGE_REGISTER_EMAIL_EXIST);
                if (model.CustomerTypeID != SystemParam.CUSTOMER_TYPE_NORMAL)
                {
                    var cusTax = await _customerRepository.GetFirstOrDefaultAsync(c => c.CodeTax.Equals(model.CodeTax) && c.IsActive.Equals(SystemParam.ACTIVE));
                    if (cusTax != null)
                        return JsonResponse.Error(SystemParam.ERROR_CODE_CODETAX_INVALID, SystemParam.MESSAGE_CODETAX_INVALID);
                }
                Customer cus = new Customer
                {
                    Avatar = model.Avatar,
                    CodeTax = model.CodeTax,
                    CustomerTypeID = model.CustomerTypeID,
                    DeviceID = model.DeviceID,
                    Email = model.Email,
                    IsConfirmRole = model.CustomerTypeID > SystemParam.CUSTOMER_TYPE_NORMAL ? SystemParam.ACTIVE_FALSE : (int?)null,
                    Password = Util.GenPass(model.Password),
                    Phone = model.Phone,
                    Name = model.Name,

                };
                await _customerRepository.AddAsync(cus);
                cus.Token = GenerateJwtToken(cus.ID.ToString(), secretKey, timeout);
                await _customerRepository.UpdateAsync(cus);
                if(model.CustomerTypeID > SystemParam.CUSTOMER_TYPE_NORMAL)
                {
                    await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_REQUEST_ROLE, string.Format(SystemParam.NOTIFICATION_TYPE_REQUEST_ROLE_STR, cus.Name), null, cus.Phone, null);
                }
                return JsonResponse.Success(cus);

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetUserInfo(int ID)
        {
            try
            {
                var cusinfo = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                return JsonResponse.Success(cusinfo);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ConfirmRole(int[] IDs)
        {
            try
            {
                for (int i = 0; i < IDs.Length; i++)
                {
                    var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(IDs[i]) && x.IsActive.Equals(SystemParam.ACTIVE),null,source => source.Include(x => x.CustomerType));
                    customer.IsConfirmRole = SystemParam.ACTIVE;
                    await _customerRepository.UpdateAsync(customer);
                    await _pushNotificationService.PushNotification(customer, SystemParam.NOTIFICATION_TYPE_CONFIRM_ROLE, string.Format(SystemParam.NOTIFICATION_TYPE_CONFIRM_ROLE_STR, customer.CustomerType.Name), null, null);
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public int? GetCustomerType(Customer model)
        {
            try
            {
                if (model != null)
                {
                    if (model.IsConfirmRole.Equals(SystemParam.ACTIVE_FALSE))
                    {
                        return null;
                    }
                    else
                    {
                        return model.CustomerTypeID;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return null;
            }

        }

        public async Task<JsonResultModel> ChangeStatus(int customerID)
        {
            try
            {
                Customer customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(customerID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (customer == null) JsonResponse.Response(SystemParam.ERROR, SystemParam.ERROR_NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER, "");
                if (customer.Status == SystemParam.ACTIVE_FALSE)
                {
                    customer.Status = SystemParam.ACTIVE;
                }
                else
                {
                    customer.Status = SystemParam.ACTIVE_FALSE;
                }

                var res = await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> GetCustomers(int page, int limit, int? customerType, int? status, string searchKey, string startDate, string endDate)
        {
            try
            {
                var list = await _customerRepository.GetCustomers(page, limit, customerType, status, searchKey, startDate, endDate);
                DataPagedListModel data = new DataPagedListModel
                {
                    Data = list,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = list.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeAvatar(Customer customer, string ImageUrl)
        {
            try
            {
                customer.Avatar = ImageUrl;
                await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateUserInfo(Customer customer, ChangeCustomerInfoModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Name) || String.IsNullOrEmpty(input.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_FIELDS_INVALID, SystemParam.MESSAGE_REGISTER_FIELDS_INVALID);
                if (!Util.ValidateEmail(input.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                var model = await _customerRepository.GetFirstOrDefaultAsync(x => x.Email.Equals(input.Email) && x.IsActive.Equals(SystemParam.ACTIVE) && !x.ID.Equals(customer.ID));
                if (model != null)
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_EXIST, SystemParam.MESSAGE_REGISTER_EMAIL_EXIST);
                customer.Name = input.Name;
                customer.Email = input.Email;
                customer.Gender = input.Gender;
                customer.DOB = Util.ConvertFromDate(input.DOB);
                await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetMemberPointHistory(int Page, int Limit, int Type, int CustomerID, string StartDate, string EndDate)
        {
            try
            {
                var model = await _memberPointHistoryRepository.GetMemberPointHistories(Page, Limit, Type, CustomerID, StartDate, EndDate);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ForgotPassword(string phone)
        {
            try
            {
                if (Util.validPhone(phone) == false) return JsonResponse.Error(SystemParam.ERROR_CODE_PHONE_NOT_FOUND, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(phone));
                if (customer == null) return JsonResponse.Error(SystemParam.ERROR_CODE_PHONE_NOT_FOUND, SystemParam.MESSAGE_PHONE_NOT_FOUND);
                if (customer.Status.Equals(SystemParam.ACTIVE_FALSE)) return JsonResponse.Error(SystemParam.ERROR_LOCK_ACOUNT, SystemParam.MESSAGE_LOCK_ACOUNT);
                if(customer.ExpireDateOTP.GetValueOrDefault() > DateTime.Now.AddDays(-1))
                {
                    customer.QtyOTP = 0;
                    await _customerRepository.UpdateAsync(customer);
                }
                if(customer.QtyOTP > SystemParam.OTP_MAX_QUANTITY)
                {
                    return JsonResponse.Error(SystemParam.ERROR_OTP_MAX_QUANTITY_EXCEED, SystemParam.MESSAGE_OTP_MAX_QUANTITY_EXCEED);
                }
                // Nếu Số điện thoại đã được đăng kí thì gửi thông báo về Email
                string Email = customer.Email;
                Random r = new Random();
                string code = r.Next(1000, 9999).ToString();
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmail = Email,
                    Body = $"Mã OTP : <b>{code}</b> từ server GiaAnhFoods có hiệu lực 5 phút. để tránh rủi ro yêu cầu khách hàng không cung cấp cho ai khác .",
                    Subject = "<###>GiaAnhFoods"
                };
                // cập nhật mã OTP vào cơ cở sữ liêu, có hiệu lực 5 phút
                customer.OTP = code;
                customer.ExpireDateOTP = DateTime.Now.AddMinutes(5);
                await _customerRepository.UpdateAsync(customer);
                //await _mail.SendEmailAsync(mailRequest);
                ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                {
                    _mail.SendEmailAsync(mailRequest);
                }));
                //Thread sentMailThread = new Thread(() =>
                //{
                //    _mail.SendEmailAsync(mailRequest);
                //});
                //sentMailThread.Start();
                var model = new
                {
                    Phone = phone,
                    Email = Email,
                    ExpireDateOTP = customer.ExpireDateOTP
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ConfirmOTP(ConfirmOTPModel model)
        {
            try
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.ACTIVE));
                if (customer == null) return JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER);
                if (customer.OTP == model.OTP && customer.ExpireDateOTP > DateTime.Now)
                {
                    customer.IsConfirmOTP = SystemParam.ACTIVE;
                    await _customerRepository.UpdateAsync(customer);
                    return JsonResponse.Success();
                }
                else
                {
                    return JsonResponse.Error(SystemParam.ERROR_CODE_INVALID_OTP, SystemParam.MESSAGE_CODE_INVALID_OTP);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ChangePasswordOTP(ChangePasswordOTPModel model)
        {
            try
            {
                var cus = await _customerRepository.GetFirstOrDefaultAsync(c => c.Phone.Equals(model.Phone) && c.IsActive.Equals(SystemParam.ACTIVE));
                if (cus == null) return JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER);
                if (cus.Status.Equals(SystemParam.ACTIVE_FALSE)) return JsonResponse.Error(SystemParam.ERROR_CODE_CUSOTMER_LOCK, SystemParam.MESSAGE_LOCK_CUSTOMER);

                if (cus.IsConfirmOTP.Equals(SystemParam.ACTIVE))
                {
                    cus.Password = Util.GenPass(model.Password);
                    cus.IsConfirmOTP = SystemParam.ACTIVE_FALSE;
                    await _customerRepository.UpdateAsync(cus);
                    return JsonResponse.Success();
                }
                else
                {
                    return JsonResponse.Error(SystemParam.ERROR_CODE_UNCONFIRMED, SystemParam.MESSAGE_UNCONFIRMED);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ChangePassword(Customer cus, string Password)
        {
            try
            {
                cus.Password = Util.GenPass(Password);
                await _customerRepository.UpdateAsync(cus);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return JsonResponse.ServerError();
            }
        }
    }


}

