
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 10/12/2021
 * Edit     : Chưa chỉnh sửa
 * Content  : Chứa các hàm Nhanh Trả về 
 * ----------------------------------*/


using APIProject.Service.Utils;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Utils
{
   public class JsonResponse
    {

        // Trạng Thái Thành Công :
        public static JsonResultModel Success(object data)
        {
            return new JsonResultModel(SystemParam.SUCCESS,SystemParam.SUCCESS_CODE , SystemParam.MESSAGE_SUCCESS, data);
        }
    
        public static JsonResultModel Success(string message,object data)
        {
            return new JsonResultModel(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE,message, data);
        }
        public static JsonResultModel Success(int code, object data)
        {
            return new JsonResultModel(SystemParam.SUCCESS,code, SystemParam.MESSAGE_SUCCESS, data);
        }
        public static JsonResultModel Success()
        {
            return new JsonResultModel(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.MESSAGE_SUCCESS,"");
        }
        // Trạng thái không thành công:
        public static JsonResultModel Error(int code,string message)
        {
            return new JsonResultModel(SystemParam.ERROR, code, message, "");
        }

        public static JsonResultModel Response(int status, int code, string message, object data)
        {
            return new JsonResultModel(status, code,message, data);
        }

        public static  JsonResultModel ServerError()
        {
            return new JsonResultModel(SystemParam.ERROR, SystemParam.ERROR_CODE,SystemParam.SERVER_ERROR, "");
        }

    }
}
