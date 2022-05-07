using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Oje.Infrastructure.Filters
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
                    context.Result = new JsonResult(new ApiResult() { errorCode = be.Code, message = context.Exception.Message, messageCode = be.BMessages });
            }
            context.ExceptionHandled = true;

        }
    }
}
