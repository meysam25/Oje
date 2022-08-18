using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;

namespace Oje.Security.Middlewares
{
    public class ValidRangeIpMiddleware
    {
        readonly RequestDelegate next = null;
        public ValidRangeIpMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, IValidRangeIpService ValidRangeIpService, IErrorService ErrorService, IInValidRangeIpService InValidRangeIpService)
        {
            var curIp = httpContext.GetIpAddress();
            if (curIp == null)
            {
                httpContext.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                await httpContext.Response.WriteAsync("<!DOCTYPE html><html lang=\"fa\"><head><meta charSet=\"utf-8\" /><title>ای پی</title></head><body dir=\"rtl\" class=\"makeRTL\"><div style='text-align:center;color:red;' >ای پی شما یافت نشد</div></body></html>");
                return;
            }

            var tempRange = ValidRangeIpService.GetCacheIpRangeList();
            if (tempRange != null && tempRange.Count > 0 &&
                !tempRange
                .Any(t =>
                            curIp.Ip1 >= t.FromIp1 && curIp.Ip1 <= t.ToIp1 && curIp.Ip2 >= t.FromIp2 && curIp.Ip2 <= t.ToIp2 &&
                            curIp.Ip3 >= t.FromIp3 && curIp.Ip3 <= t.ToIp3 && curIp.Ip4 >= t.FromIp4 && curIp.Ip4 <= t.ToIp4
                    )
                )
            {
                //throw BException.GenerateNewException(String.Format(BMessages.Please_Dont_Use_VPN.GetEnumDisplayName(), curIp.ToString()));

                if (httpContext.Request.Headers.ContainsKey("X-Requested-With"))
                {
                    httpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    await httpContext.Response.WriteAsync("{ \"isSuccess\": false, \"message\": \"لطفا وی پی ان خود را خاموش کنید " + curIp.ToString() + "\" }");
                }
                else
                {
                    httpContext.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    await httpContext.Response.WriteAsync("<!DOCTYPE html><html style=\"height:100vh\" lang=\"fa\"><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no\"><meta charSet=\"utf-8\" /><title>وی پی ان</title></head><body style=\"height:100vh;display:flex;justify-content:center;-webkit-justify-content:center;align-items:center;-webkit-align-items:center;\" dir=\"rtl\" class=\"makeRTL\"><div style='text-align:center;color:red;margin-top:-100px;' >لطفا وی پی ان خود را خاموش کنید, درصورتی که از خاموش بودن وی پی ان خود اطمینان دارید لطفا 5 دقیقه دیگر دوباره امتحان کنید" + curIp.ToString() + "</div></body></html>");
                }
               
                if (InValidRangeIpService.Create(curIp))
                    ErrorService.Create(httpContext.GetLoginUser()?.UserId, httpContext.TraceIdentifier, Infrastructure.Enums.ApiResultErrorCode.ValidationError, BMessages.Please_Dont_Use_VPN, String.Format(BMessages.Please_Dont_Use_VPN.GetEnumDisplayName(), curIp.ToString()), curIp, "37", "ValidRangeIpMiddleware.cs", "", "");
                return;
            }


            await next(httpContext);
        }
    }

    public static class ValidRangeIpMiddlewareExtensions
    {
        public static IApplicationBuilder PreventIps(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidRangeIpMiddleware>();
        }
    }
}
