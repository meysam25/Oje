using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class ApiResult
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public ApiResultErrorCode errorCode { get; set; }
        public BMessages messageCode { get; set; }
        public object data { get; set; }
        public string fileData { get; set; }
        public string fileName { get; set; }

        public static ApiResult GenerateNewResult(bool isSuccess, BMessages messageCode)
        {
            return new ApiResult() { isSuccess = isSuccess, messageCode = messageCode, message = messageCode.GetAttribute<DisplayAttribute>()?.Name };
        }

        public static ApiResult GenerateNewResult(bool isSuccess, BMessages messageCode, object data)
        {
            return new ApiResult() { isSuccess = isSuccess, messageCode = messageCode, message = messageCode.GetAttribute<DisplayAttribute>()?.Name, data = data };
        }

        public static ApiResult GenerateNewResult(bool isSuccess, BMessages messageCode, string fileData, string fileName)
        {
            return new ApiResult() { isSuccess = isSuccess, messageCode = messageCode, message = messageCode.GetAttribute<DisplayAttribute>()?.Name, fileData = fileData, fileName = fileName };
        }
    }
}
