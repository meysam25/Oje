using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if ((context.HttpContext.Request.Host.Host + ":" + (context.HttpContext.Request.Host.Port == null ? 80 : context.HttpContext.Request.Host.Port)) != referUrl && referUrl != "rt.sizpay.ir:443")
                    throw BException.GenerateNewException(BMessages.ReferrUrl_Is_Invalid);
            }
        }
    }
}
