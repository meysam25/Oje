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
    public class BlockAutoIpService : IBlockAutoIpService
    {
        readonly SecurityDBContext db = null;
        readonly IBlockClientConfigService BlockClientConfigService = null;
        readonly IBlockFirewallIpService BlockFirewallIpService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        static object lockObj = null;

        public BlockAutoIpService
            (
                SecurityDBContext db,
                IBlockClientConfigService BlockClientConfigService,
                IBlockFirewallIpService BlockFirewallIpService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.BlockClientConfigService = BlockClientConfigService;
            this.BlockFirewallIpService = BlockFirewallIpService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public void CheckIfRequestIsValid(BlockClientConfigType type, BlockAutoIpAction exeType, IpSections ipSections, int? siteSettingId)
        {
            BlockClientConfig foundConfig = BlockClientConfigService.GetBy(type, siteSettingId);
            if (foundConfig != null)
            {
                if (exeType == BlockAutoIpAction.BeforeExecute)
                    validate(ipSections, siteSettingId, foundConfig.MaxFirewall, foundConfig.MaxSoftware, type, exeType);
                else if (exeType == BlockAutoIpAction.AfterExecute)
                    validate(ipSections, siteSettingId, foundConfig.MaxSuccessFirewall, foundConfig.MaxSuccessSoftware, type, exeType);
            }
        }

        private void createNewItem(IpSections ipSections, int? siteSettingId, DateTime now, BlockClientConfigType type, BlockAutoIpAction exeType)
        {
            if (lockObj == null)
                lockObj = new Object();

            lock (lockObj)
            {
                if (!db.BlockAutoIps
                        .Any(
                                t => t.Ip4 == ipSections.Ip4 && t.Ip3 == ipSections.Ip3 && t.Ip2 == ipSections.Ip2 && t.Ip1 == ipSections.Ip1 &&
                                t.BlockClientConfigType == type && t.CreateDate == now && t.BlockAutoIpAction == exeType
                            )
                   )
                {
                    db.Entry(new BlockAutoIp()
                    {
                        BlockClientConfigType = type,
                        CreateDate = now,
                        CreateDay = now.Day,
                        CreateMonth = now.Month,
                        CreateYear = now.Year,
                        Ip1 = ipSections.Ip1,
                        Ip2 = ipSections.Ip2,
                        Ip3 = ipSections.Ip3,
                        Ip4 = ipSections.Ip4,
                        BlockAutoIpAction = exeType,
                        SiteSettingId = siteSettingId.Value,
                        RequestId = HttpContextAccessor.HttpContext.TraceIdentifier,
                        UserId = HttpContextAccessor.HttpContext.GetLoginUser()?.UserId
                    }).State = EntityState.Added;
                    db.SaveChanges();
                }
            }

        }

        void validate(IpSections ipSections, int? siteSettingId, int maxFirewall, int maxSoftware, BlockClientConfigType type, BlockAutoIpAction exeType)
        {
            var dtKnow = DateTime.Now;
            var countExist = db.BlockAutoIps
                .Where(t =>
                        t.SiteSettingId == siteSettingId && t.BlockAutoIpAction == exeType &&
                        t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4 &&
                        t.CreateYear == dtKnow.Year && t.CreateMonth == dtKnow.Month && t.CreateDay == dtKnow.Day && t.BlockClientConfigType == type
                    )
                .Count();
            if (countExist > maxFirewall)
            {
                BlockFirewallIpService.Create(ipSections, siteSettingId, type);
            }
            if (countExist > maxSoftware)
            {
                if (maxFirewall > 0)
                    createNewItem(ipSections, siteSettingId, dtKnow, type, exeType);
                throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
            }
            else
                createNewItem(ipSections, siteSettingId, dtKnow, type, exeType);
        }

        public GridResultVM<BlockAutoIpMainGridResultVM> GetList(BlockAutoIpMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new BlockAutoIpMainGrid();

            var tempDate = Convert.ToDateTime("2000/01/01");

            var quiryResult = db.BlockAutoIps.Where(t => t.SiteSettingId == siteSettingId && t.BlockAutoIpAction == BlockAutoIpAction.BeforeExecute);

            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var targetIp = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == targetIp.Ip1 && t.Ip2 == targetIp.Ip2 && t.Ip3 == targetIp.Ip3 && t.Ip4 == targetIp.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.fullUsername))
                quiryResult = quiryResult.Where(t => (t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "").Contains(searchInput.fullUsername));
            if (searchInput.section != null)
                quiryResult = quiryResult.Where(t => t.BlockClientConfigType == searchInput.section);
            if(searchInput.isSuccess == true)
                quiryResult = quiryResult.Where(t => !db.Errors.Any(tt => tt.RequestId == t.RequestId));
            else if (searchInput.isSuccess == false)
                quiryResult = quiryResult.Where(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));

            switch (searchInput.sortField)
            {
                case "section":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.BlockClientConfigType);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.BlockClientConfigType);
                    break;
                case "ip":
                    break;
                case "createDate":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    break;
                case "isSuccess":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));
                    else
                        quiryResult = quiryResult.OrderBy(t => db.Errors.Any(tt => tt.RequestId == t.RequestId));
                    break;
                case "fullUsername":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "");
                    else
                        quiryResult = quiryResult.OrderBy(t => t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "");
                    break;
                default:
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    break;
            }

            int row = searchInput.skip;

            return new GridResultVM<BlockAutoIpMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.BlockClientConfigType,
                    t.CreateDate,
                    t.BlockAutoIpAction,
                    fullUsername = t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "",
                    endDate = db.BlockAutoIps.Any(tt => tt.RequestId == t.RequestId && tt.BlockAutoIpAction == BlockAutoIpAction.AfterExecute) ? db.BlockAutoIps.Where(tt => tt.RequestId == t.RequestId && tt.BlockAutoIpAction == BlockAutoIpAction.AfterExecute).Select(tt => tt.CreateDate).FirstOrDefault() : tempDate,
                    isSuccess = !db.Errors.Any(tt => tt.RequestId == t.RequestId),
                    t.RequestId
                })
                .ToList()
                .Select(t => new BlockAutoIpMainGridResultVM
                {
                    row = ++row,
                    id = t.Ip1 + "_" + t.Ip2 + "_" + t.Ip3 + "_" + t.Ip4 + "_" + t.BlockClientConfigType + "_" + t.CreateDate.Ticks + "_" + t.BlockAutoIpAction,
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("HH:mm"),
                    fullUsername = t.fullUsername,
                    section = t.BlockClientConfigType.GetEnumDisplayName(),
                    duration = t.endDate != tempDate ? (t.endDate - t.CreateDate).TotalMilliseconds + " ms" : "",
                    isSuccess = t.isSuccess == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    rid = t.RequestId
                })
                .ToList()
            };
        }
    }
}
