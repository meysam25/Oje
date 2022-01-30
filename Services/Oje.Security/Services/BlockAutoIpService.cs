using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Services
{
    public class BlockAutoIpService : IBlockAutoIpService
    {
        readonly SecurityDBContext db = null;
        readonly IBlockClientConfigService BlockClientConfigService = null;
        readonly IBlockFirewallIpService BlockFirewallIpService = null;
        public BlockAutoIpService
            (
                SecurityDBContext db,
                IBlockClientConfigService BlockClientConfigService,
                IBlockFirewallIpService BlockFirewallIpService
            )
        {
            this.db = db;
            this.BlockClientConfigService = BlockClientConfigService;
            this.BlockFirewallIpService = BlockFirewallIpService;
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
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        void validate(IpSections ipSections, int? siteSettingId, int maxFirewall, int maxSoftware, BlockClientConfigType type, BlockAutoIpAction exeType)
        {
            var dtKnow = DateTime.Now;
            var countExist = db.BlockAutoIps
                .Where(t =>
                        t.SiteSettingId == siteSettingId && t.BlockAutoIpAction == exeType &&
                        t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4 &&
                        t.CreateYear == dtKnow.Year && t.CreateMonth == dtKnow.Month && t.CreateDay == dtKnow.Day
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
    }
}
