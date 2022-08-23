using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;

namespace Oje.Infrastructure.Filters
{
    public class PreventPostRequestIfNoReferrerUrlFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "POST")
            {
                var referUrl = context.HttpContext.Request.GetRefererUrl();
                if (string.IsNullOrEmpty(referUrl))
                    throw BException.GenerateNewException(BMessages.ReferrUrl_Is_Invalid);
                if ((context.HttpContext.Request.Host.Host ) != referUrl && referUrl != "rt.sizpay.ir:443" && referUrl != "rt.sizpay.ir" && referUrl != "sep.shaparak.ir" && referUrl != "sadad.shaparak.ir" && referUrl != "195.110.39.70")
                    throw BException.GenerateNewException(BMessages.ReferrUrl_Is_Invalid);
            }
        }
    }
}
