using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using System.Diagnostics;

namespace Oje.Security.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public CustomExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            string ajaxStr = "";
            if (context.HttpContext.Request.Headers.ContainsKey("X-Requested-With"))
                ajaxStr = context.HttpContext.Request.Headers["X-Requested-With"];

            BException be = context.Exception as BException;

            logError(context, be);

            if (string.IsNullOrEmpty(ajaxStr))
            {
                var result = new ViewResult { ViewName = "CustomError" };
                result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                if (be == null)
                    result.ViewData.Add("Exception", "خطا در انجام عملیات");
                else
                    result.ViewData.Add("Exception", be.Message);
                context.Result = result;
            }
            else
            {

                //context.Result = new JsonResult(new ApiResult() { errorCode = ApiResultErrorCode.UnknownError, message = context.Exception.Message + " " + context.Exception?.InnerException?.Message + " " + context.Exception?.InnerException?.InnerException?.Message});
                if (be == null)
                    context.Result = new JsonResult(new ApiResult() { errorCode = ApiResultErrorCode.UnknownError, message = "خطا در انجام عملیات" });
                else
                    context.Result = new JsonResult(new ApiResult() { errorCode = 0, message = context.Exception.Message, messageCode = 0 });
            }
            context.ExceptionHandled = true;

        }

        private void logError(ExceptionContext context, BException be)
        {
            string cMessages = " ";
            string cLineNumbers = " ";
            string cFilenames = " ";
            var st = new StackTrace(context.Exception, true);
            for (var i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                int line = frame.GetFileLineNumber();
                string filename = frame.GetFileName();
                if (line > 0)
                    cLineNumbers += line + Environment.NewLine;
                if (!string.IsNullOrEmpty(filename))
                    cFilenames += filename + Environment.NewLine;
            }

            Exception tempEx = context.Exception;
            while (tempEx != null)
            {
                cMessages += tempEx.Message + Environment.NewLine;
                tempEx = tempEx.InnerException;
            }

            var curIp = context.HttpContext.GetIpAddress();
            if (curIp != null)
            {
                IErrorService ErrorService = context.HttpContext.RequestServices.GetService(typeof(IErrorService)) as IErrorService;
                if (ErrorService != null)
                    ErrorService.Create(context.HttpContext.GetLoginUser()?.UserId, context.HttpContext.TraceIdentifier, be?.Code, be?.BMessages, cMessages, curIp, cLineNumbers, cFilenames);
            }
        }
    }
}
