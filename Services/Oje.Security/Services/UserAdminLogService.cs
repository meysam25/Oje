using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class UserAdminLogService : IUserAdminLogService
    {
        readonly SecurityDBContext db = null;
        readonly IAdminBlockClientConfigService AdminBlockClientConfigService = null;

        public UserAdminLogService
            (
                SecurityDBContext db,
                IAdminBlockClientConfigService AdminBlockClientConfigService
            )
        {
            this.db = db;
            this.AdminBlockClientConfigService = AdminBlockClientConfigService;
        }

        public void Create(long loginUserId, string requestId, long actionId, bool isSuccess, bool isStart, IpSections ip, int siteSettingId)
        {
            if (loginUserId > 0 && !string.IsNullOrEmpty(requestId) && requestId.Length <= 50 && actionId > 0 && ip != null && siteSettingId > 0)
            {
                if (isStart == true)
                {
                    var foundExecuteLimit = AdminBlockClientConfigService.GetByFromCache(actionId, siteSettingId);
                    if (foundExecuteLimit != null)
                        validationTodayActivity(loginUserId, foundExecuteLimit.MaxValue, ip, siteSettingId, actionId);
                }

                db.Entry(new UserAdminLog()
                {
                    UserId = loginUserId,
                    ActionId = actionId,
                    CreateDate = DateTime.Now,
                    Ip1 = ip.Ip1,
                    Ip2 = ip.Ip2,
                    Ip3 = ip.Ip3,
                    Ip4 = ip.Ip4,
                    IsStart = isStart,
                    SiteSettingId = siteSettingId,
                    IsSuccess = isSuccess,
                    RequestId = requestId
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        private void validationTodayActivity(long loginUserId, int maxValue, IpSections ip, int siteSettingId, long actionId)
        {
            if (db.UserAdminLogs.Count(t => t.ActionId == actionId && t.UserId == loginUserId && t.SiteSettingId == siteSettingId && t.Ip1 == ip.Ip1 && t.Ip2 == ip.Ip2 && t.Ip3 == ip.Ip3 && t.Ip4 == ip.Ip4 && t.IsStart == true) >= maxValue)
                throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
        }

        public GridResultVM<UserAdminLogMainGridResultVM> GetList(UserAdminLogMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserAdminLogMainGrid();

            var quiryResult = db.UserAdminLogs.Where(t => t.SiteSettingId == siteSettingId && t.IsStart == true);

            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.action))
                quiryResult = quiryResult.Where(t => (t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title).Contains(searchInput.action));
            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var ipSection = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == ipSection.Ip1 && t.Ip2 == ipSection.Ip2 && t.Ip3 == ipSection.Ip3 && t.Ip4 == ipSection.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.isSuccess == true)
                quiryResult = quiryResult.Where(t => !db.Errors.Any(tt => tt.RequestId == t.RequestId));
            else if (searchInput.isSuccess == false)
                quiryResult = quiryResult.Where(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));


            var tempDate = Convert.ToDateTime("1900/01/01");

            bool hasSort = false;
            if (!string.IsNullOrEmpty(searchInput.sortField))
            {
                if (searchInput.sortField == "userFullname" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname);
                    hasSort = true;
                }
                else if (searchInput.sortField == "userFullname" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Username : t.User.Firstname + " " + t.User.Lastname);
                    hasSort = true;
                }
                else if (searchInput.sortField == "action" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Action.Controller.Section.Title + t.Action.Controller.Title + t.Action.Title);
                    hasSort = true;
                }
                else if (searchInput.sortField == "action" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Action.Controller.Section.Title + t.Action.Controller.Title + t.Action.Title);
                    hasSort = true;
                }
                else if (searchInput.sortField == "ip" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Ip1).ThenByDescending(t => t.Ip2).ThenByDescending(t => t.Ip3).ThenByDescending(t => t.Ip4);
                    hasSort = true;
                }
                else if (searchInput.sortField == "ip" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Ip1).ThenBy(t => t.Ip2).ThenBy(t => t.Ip3).ThenBy(t => t.Ip4);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "isSuccess" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));
                    hasSort = true;
                }
                else if (searchInput.sortField == "isSuccess" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));
                    hasSort = true;
                }
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);


            int row = searchInput.skip;

            return new GridResultVM<UserAdminLogMainGridResultVM>()
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
                    action = t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title,
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    cDate = t.CreateDate,
                    endCreateDate = db.UserAdminLogs.Any(tt => tt.RequestId == t.RequestId && tt.IsStart == false) ? db.UserAdminLogs.Where(tt => tt.RequestId == t.RequestId && tt.IsStart == false).Select(tt => tt.CreateDate).FirstOrDefault() : tempDate,
                    hasError = db.Errors.Where(tt => tt.RequestId == t.RequestId).Any(),
                    rid = t.RequestId
                })
                .ToList()
                .Select(t => new UserAdminLogMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    userFullname = string.IsNullOrEmpty((t.Firstname + " " + t.Lastname).Trim()) ? t.Username : t.Firstname + " " + t.Lastname,
                    action = t.action,
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    createDate = t.cDate.ToFaDate() + " " + t.cDate.ToString("HH:mm"),
                    duration = t.endCreateDate != tempDate ? ((t.endCreateDate - t.cDate).TotalMilliseconds + " ms") : "",
                    isSuccess = t.hasError == true ? BMessages.No.GetEnumDisplayName() : BMessages.Yes.GetEnumDisplayName(),
                    rid = t.rid
                })
                .ToList()
            };
        }
    }
}
