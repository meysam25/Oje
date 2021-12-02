using Oje.AccountManager.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Oje.AccountManager.Models.View;

namespace Oje.AccountManager.Filters
{
    public class CustomeAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        public static List<UserAccessCache> UserAccessCaches = new();
        public CustomeAuthorizeFilter()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isValidUserr = false;
            string loginCValue = "";
            bool noAccess = false;
            LoginUserVM loginUser = null;
            string requestPath =
                (context.HttpContext.Request.RouteValues.ContainsKey("area") ? "/" + context.HttpContext.Request.RouteValues["area"] : "") +
                (context.HttpContext.Request.RouteValues.ContainsKey("controller") ? "/" + context.HttpContext.Request.RouteValues["controller"] : "") +
                (context.HttpContext.Request.RouteValues.ContainsKey("action") ? "/" + context.HttpContext.Request.RouteValues["action"] : "");
            IUserManager UserManager = context.HttpContext.RequestServices.GetService(typeof(IUserManager)) as IUserManager;
            ISiteSettingManager SiteSettingManager = context.HttpContext.RequestServices.GetService(typeof(ISiteSettingManager)) as ISiteSettingManager;
            var foundSetting = SiteSettingManager.GetSiteSetting();
            if (foundSetting != null && context.HttpContext.Request.Cookies.ContainsKey("login"))
            {
                loginCValue = context.HttpContext.Request.Cookies["login"];
                loginUser = loginCValue.Decrypt2AndGetUserVM();
                if (loginUser != null)
                {
                    if (loginUser.Ip == context.HttpContext.Connection.RemoteIpAddress.ToString() && (loginUser.siteSettingId == null || loginUser.siteSettingId == foundSetting.Id))
                    {
                        if (!UserAccessCaches.Any(t => t.UserId == loginUser.UserId) ||
                            UserAccessCaches.Where(t => t.UserId == loginUser.UserId && (DateTime.Now - t.CreateDate).TotalMinutes > 10).FirstOrDefault() != null)
                        {
                            var userAccess = UserManager.GetUserSections(loginUser.UserId);
                            var foundItem = UserAccessCaches.Where(t => t.UserId == loginUser.UserId).FirstOrDefault();
                            if (foundItem == null)
                                UserAccessCaches.Add(new UserAccessCache() { UserId = loginUser.UserId, Actions = userAccess });
                            else
                                foundItem.Actions = userAccess;
                        }
                        var foundUserAccess = UserAccessCaches.Where(t => t.UserId == loginUser.UserId).FirstOrDefault();
                        if (foundUserAccess.Actions.Any(t => t.Name == requestPath))
                        {
                            isValidUserr = true;
                        }
                        else
                        {
                            noAccess = true;
                        }
                    }
                }
            }
            if (isValidUserr == false)
            {
                if (context.HttpContext.Request.Headers.ContainsKey("X-Requested-With") == true)
                {
                    if (noAccess == false)
                        context.Result = new JsonResult(new ApiResult() { errorCode = ApiResultErrorCode.NeedLoginFist, message = "لطفا ابتدا لاگین کنید", isSuccess = false });
                    else
                        context.Result = new JsonResult(new ApiResult() { errorCode = ApiResultErrorCode.UnauthorizeAccess, message = "دسترسی شما به این بخش محدود می باشد", isSuccess = false });
                }
                else
                {
                    if (noAccess == false)
                        context.Result = new RedirectToActionResult("Login", "Dashboard", new { area = "Account" });
                    else
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                }
            }
        }
    }
}
