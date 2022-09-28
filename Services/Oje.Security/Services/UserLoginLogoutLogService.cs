using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class UserLoginLogoutLogService : IUserLoginLogoutLogService
    {
        readonly SecurityDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserLoginLogoutLogService
            (
                SecurityDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public void Create(long userId, UserLoginLogoutLogType type, int? siteSettingId, bool isSuccess, string message)
        {
            var curIp = HttpContextAccessor.HttpContext.GetIpAddress();
            if (userId > 0 && siteSettingId.ToIntReturnZiro() > 0 && curIp != null && !string.IsNullOrEmpty(message))
            {
                db.Entry(new UserLoginLogoutLog()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    SiteSettingId = siteSettingId.Value,
                    CreateDate = DateTime.Now,
                    Type = type,
                    Ip1 = curIp.Ip1,
                    Ip2 = curIp.Ip2,
                    Ip3 = curIp.Ip3,
                    Ip4 = curIp.Ip4,
                    IsSuccess = isSuccess,
                    Message = message
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public GridResultVM<UserLoginLogoutLogMainGridResultVM> GetList(UserLoginLogoutLogMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserLoginLogoutLogMainGrid();

            var quiryResult = db.UserLoginLogoutLogs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.userfullname))
                quiryResult = quiryResult
                    .Where(t => (string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userfullname));
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.message))
                quiryResult = quiryResult.Where(t => t.Message.Contains(searchInput.message));
            if (searchInput.isSuccess != null)
                quiryResult = quiryResult.Where(t => t.IsSuccess == searchInput.isSuccess);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var curIp = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == curIp.Ip1 && t.Ip2 == curIp.Ip2 && t.Ip3 == curIp.Ip3 && t.Ip4 == curIp.Ip4);
            }

            switch (searchInput.sortField)
            {
                case "userfullname":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => (string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname));
                    else
                        quiryResult = quiryResult.OrderBy(t => (string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname));
                    break;
                case "createDate":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    break;
                case "type":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Type);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Type);
                    break;
                case "ip":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Ip1).ThenByDescending(t => t.Ip2).ThenByDescending(t => t.Ip3).ThenByDescending(t => t.Ip4);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Ip1).ThenBy(t => t.Ip2).ThenBy(t => t.Ip3).ThenBy(t => t.Ip4);
                    break;
                case "message":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Message);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Message);
                    break;
                case "isSuccess":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.IsSuccess);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.IsSuccess);
                    break;
                default:
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    break;
            }

            int row = searchInput.skip;

            return new GridResultVM<UserLoginLogoutLogMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.User.Firstname,
                    t.User.Lastname,
                    t.User.Username,
                    t.CreateDate,
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.Type,
                    t.Message,
                    t.IsSuccess
                })
                .ToList()
                .Select(t => new UserLoginLogoutLogMainGridResultVM
                {
                    row = ++row,
                    id = t.id + "",
                    userfullname = (string.IsNullOrEmpty((t.Firstname + " " + t.Lastname).Trim()) ? t.Username : t.Firstname + " " + t.Lastname),
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("HH:mm"),
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    type = t.Type.GetEnumDisplayName(),
                    message = t.Message,
                    isSuccess = t.IsSuccess == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
