using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Oje.AccountService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Middlewares
{
    public class HttpsHeadersMiddleware
    {
        readonly RequestDelegate next;
        public HttpsHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, ISiteSettingService SiteSettingService, IHttpContextAccessor HttpContextAccessor)
        {
            var foundSiteSetting = SiteSettingService.GetSiteSetting();
            if (foundSiteSetting == null)
            {
                await httpContext.Response.WriteAsync("setting not found");
                return;
            }
            if (foundSiteSetting.IsHttps == true && httpContext.Request.IsHttps != true)
            {
                if (httpContext.Request.Protocol.ToLower() == "get")
                    httpContext.Response.Redirect("https://" + foundSiteSetting.WebsiteUrl + httpContext.Request.Path + httpContext.Request.QueryString);
                else
                    httpContext.Response.Redirect("https://" + foundSiteSetting.WebsiteUrl);
                return;
            }
            if (foundSiteSetting.IsHttps == true)
            {
                string domainUrl = "https://" + foundSiteSetting.WebsiteUrl;

                if (!httpContext.Response.Headers.Keys.Contains("Strict-Transport-Security"))
                    httpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=2592000");
                if (!httpContext.Response.Headers.Keys.Contains("Content-Security-Policy"))
                    httpContext.Response.Headers.Add("Content-Security-Policy",
                        "default-src wss: 'self' data: " + domainUrl + " https:;style-src 'unsafe-inline' 'self' " +
                        domainUrl + " https:;script-src 'self' " + domainUrl + " 'unsafe-inline' 'unsafe-eval' https:; img-src " + domainUrl + " data: https:");
                if (!httpContext.Response.Headers.Keys.Contains("X-Content-Type-Options"))
                    httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                if (!httpContext.Response.Headers.Keys.Contains("X-XSS-Protection"))
                    httpContext.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                if (!httpContext.Response.Headers.Keys.Contains("Referrer-Policy"))
                    httpContext.Response.Headers.Add("Referrer-Policy", "strict-origin");
                if (!httpContext.Response.Headers.Keys.Contains("X-Permitted-Cross-Domain-Policies"))
                    httpContext.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                if (!httpContext.Response.Headers.Keys.Contains("X-FRAME-OPTIONS"))
                    httpContext.Response.Headers.Add("X-FRAME-OPTIONS", "SAMEORIGIN");
                if (!httpContext.Response.Headers.Keys.Contains("Permissions-Policy"))
                    httpContext.Response.Headers.Add("Permissions-Policy", "geolocation=(self)");
            }

            await next(httpContext);
        }
    }

    public static class HttpsHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder HttpsStuff(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpsHeadersMiddleware>();
        }
    }
}
