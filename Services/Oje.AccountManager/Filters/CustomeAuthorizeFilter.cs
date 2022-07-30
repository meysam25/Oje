using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Oje.AccountService.Models.View;
using Oje.Security.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Oje.AccountService.Filters
{
    public class CustomeAuthorizeFilter : Attribute, IAuthorizationFilter, IActionFilter
    {
        public static List<UserAccessCache> UserAccessCaches = new();

        public CustomeAuthorizeFilter()
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var currentFilter = context.ActionDescriptor.FilterDescriptors.FirstOrDefault(filterDescriptor => ReferenceEquals(filterDescriptor.Filter, this));
            //if (currentFilter == null)
            //    return;

            //if (currentFilter.Scope == FilterScope.Action )
            //{
            var curIp = context.HttpContext.GetIpAddress();
            if (curIp != null)
            {
                IUserAdminLogService UserAdminLogService = context.HttpContext.RequestServices.GetService(typeof(IUserAdminLogService)) as IUserAdminLogService;
                IUserAdminLogConfigService UserAdminLogConfigService = context.HttpContext.RequestServices.GetService(typeof(IUserAdminLogConfigService)) as IUserAdminLogConfigService;
                IUserService UserService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                LoginUserVM loginUser = context.HttpContext.GetLoginUser();
                string requestPath = getCurrPath(context.HttpContext);
                if (loginUser != null)
                {
                    fillUserCache(loginUser?.UserId ?? 0, UserService);
                    var foundUserAccess = UserAccessCaches.Where(t => t.UserId == loginUser.UserId).FirstOrDefault();
                    if (foundUserAccess != null)
                    {
                        var foundActionId = foundUserAccess.Actions.Where(t => t.Name == requestPath).Select(t => t.Id).FirstOrDefault();
                        if (foundActionId > 0 && UserAdminLogConfigService.IsNeededCache(foundActionId, loginUser.siteSettingId.ToIntReturnZiro()))
                            UserAdminLogService.Create(loginUser.UserId, context.HttpContext.TraceIdentifier, foundActionId, false, true, curIp, loginUser.siteSettingId.ToIntReturnZiro());
                    }
                }
            }
            // }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //var currentFilter = context.ActionDescriptor.FilterDescriptors.FirstOrDefault(filterDescriptor => ReferenceEquals(filterDescriptor.Filter, this));
            //if (currentFilter == null)
            //    return;

            //if (currentFilter.Scope == FilterScope.Action)
            //{
            var curIp = context.HttpContext.GetIpAddress();
            if (curIp != null)
            {
                IUserAdminLogService UserAdminLogService = context.HttpContext.RequestServices.GetService(typeof(IUserAdminLogService)) as IUserAdminLogService;
                IUserAdminLogConfigService UserAdminLogConfigService = context.HttpContext.RequestServices.GetService(typeof(IUserAdminLogConfigService)) as IUserAdminLogConfigService;
                IUserService UserService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                LoginUserVM loginUser = context.HttpContext.GetLoginUser();
                string requestPath = getCurrPath(context.HttpContext);
                if (loginUser != null)
                {
                    fillUserCache(loginUser?.UserId ?? 0, UserService);
                    var foundUserAccess = UserAccessCaches.Where(t => t.UserId == loginUser.UserId).FirstOrDefault();
                    if (foundUserAccess != null)
                    {
                        var foundActionId = foundUserAccess.Actions.Where(t => t.Name == requestPath).Select(t => t.Id).FirstOrDefault();
                        if (foundActionId > 0 && UserAdminLogConfigService.IsNeededCache(foundActionId, loginUser.siteSettingId.ToIntReturnZiro()))
                            UserAdminLogService.Create(loginUser.UserId, context.HttpContext.TraceIdentifier, foundActionId, false, false, curIp, loginUser.siteSettingId.ToIntReturnZiro());
                    }
                }
            }
            //}
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isValidUserr = false;
            string loginCValue = "";
            bool noAccess = false;
            LoginUserVM loginUser = null;
            string requestPath = getCurrPath(context.HttpContext);

            IUserService UserService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
            ISiteSettingService SiteSettingService = context.HttpContext.RequestServices.GetService(typeof(ISiteSettingService)) as ISiteSettingService;
            IUserLoginConfigService UserLoginConfigService = context.HttpContext.RequestServices.GetService(typeof(IUserLoginConfigService)) as IUserLoginConfigService;
            var foundSetting = SiteSettingService.GetSiteSetting();
            if (foundSetting != null && context.HttpContext.Request.Cookies.ContainsKey("login"))
            {
                loginCValue = context.HttpContext.Request.Cookies["login"];
                loginUser = loginCValue.Decrypt2AndGetUserVM();
                if (loginUser != null)
                {
                    bool ignoreIp = false;

                    if (context.HttpContext.Request.Cookies.ContainsKey("ignoreCIP"))
                    {
                        try { var tempResultX = context.HttpContext.Request.Cookies["ignoreCIP"]; ignoreIp = tempResultX.Decrypt2() == "true" ? true : false; } catch { }
                    }

                    if (loginUser.browserName == context.HttpContext.GetBroswerName())
                    {
                        if ((ignoreIp == true || loginUser.Ip == context.HttpContext.GetIpAddress()?.ToString()) && (loginUser.siteSettingId == null || loginUser.siteSettingId == foundSetting.Id))
                        {
                            fillUserCache(loginUser?.UserId ?? 0, UserService);
                            var foundUserAccess = UserAccessCaches.Where(t => t.UserId == loginUser.UserId).FirstOrDefault();
                            if (foundUserAccess != null)
                                if (foundUserAccess.Actions.Any(t => t.Name == requestPath))
                                    isValidUserr = true;
                                else
                                    noAccess = true;

                            var loginUserConfig = UserLoginConfigService.GetByCache(foundSetting?.Id);
                            if (loginUserConfig != null)
                            {
                                if (foundUserAccess != null)
                                    if ((DateTime.Now - foundUserAccess.LastActiveTime).TotalMinutes >= loginUserConfig.InActiveLogoffMinute)
                                    {
                                        isValidUserr = false;
                                        noAccess = false;
                                        MySession.Clean(loginUser.sessionFileName);
                                        foundUserAccess.CreateDate = DateTime.Now.AddDays(-1);
                                    }
                                    else
                                        foundUserAccess.LastActiveTime = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        isValidUserr = false;
                        noAccess = false;
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
                        context.Result = new RedirectToActionResult("Login", "Dashboard", new { area = "Account", returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString + "" });
                    else
                        context.Result = new ViewResult { ViewName = "AccessDenied" };
                }

                if (noAccess == true && loginUser != null && loginUser.UserId > 0)
                    UserService.CreateUserAccessRequest(loginUser.UserId, requestPath);
            }
        }

        private string getCurrPath(HttpContext context)
        {
            return (context.Request.RouteValues.ContainsKey("area") ? "/" + context.Request.RouteValues["area"] : "") +
                (context.Request.RouteValues.ContainsKey("controller") ? "/" + context.Request.RouteValues["controller"] : "") +
                (context.Request.RouteValues.ContainsKey("action") ? "/" + context.Request.RouteValues["action"] : "");
        }

        static void fillUserCache(long UserId, IUserService UserService)
        {
            if (UserId <= 0)
                return;
            if (UserAccessCaches == null)
                UserAccessCaches = new List<UserAccessCache>();

            if (!UserAccessCaches.Any(t => t.UserId == UserId) ||
                UserAccessCaches.Where(t => t.UserId == UserId && (DateTime.Now - t.CreateDate).TotalMinutes > 10).FirstOrDefault() != null)
            {
                var userAccess = UserService.GetUserSections(UserId);
                var foundItem = UserAccessCaches.Where(t => t.UserId == UserId).FirstOrDefault();
                if (foundItem == null)
                    UserAccessCaches.Add(new UserAccessCache() { UserId = UserId, Actions = userAccess, LastActiveTime = DateTime.Now });
                else
                {
                    foundItem.Actions = userAccess;
                    foundItem.LastActiveTime = DateTime.Now;
                    foundItem.CreateDate = DateTime.Now;
                }
            }
        }

        internal static void CleanCacheByUserId(long userId, IUserService UserService)
        {
            if (UserAccessCaches != null)
                fillUserCache(userId, UserService);
        }
    }
}
