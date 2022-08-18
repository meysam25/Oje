using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class ErrorFirewallManualAddService: IErrorFirewallManualAddService
    {
        readonly SecurityDBContext db = null;
        readonly IBlockFirewallIpService BlockFirewallIpService = null;

        public ErrorFirewallManualAddService
            (
                SecurityDBContext db,
                IBlockFirewallIpService BlockFirewallIpService
            )
        {
            this.db = db;
            this.BlockFirewallIpService = BlockFirewallIpService;
        }

        public ApiResult Block(string ip, int? siteSettingId)
        {
            var ipConfig = ip.GetIpSections();

            if (ipConfig == null)
                throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);

            if (db.ErrorFirewallManualAdds.Any(t => t.Ip4 == ipConfig.Ip1 && t.Ip2 == ipConfig.Ip2 && t.Ip3 == ipConfig.Ip3 && t.Ip4 == ipConfig.Ip4))
                throw BException.GenerateNewException(BMessages.Dublicate_IP);

            db.Entry(new ErrorFirewallManualAdd { CreateDate = DateTime.Now, Ip1 = ipConfig.Ip1, Ip2 = ipConfig.Ip2, Ip3 = ipConfig.Ip3, Ip4 = ipConfig.Ip4 }).State = EntityState.Added;
            db.SaveChanges();

            BlockFirewallIpService.Create(ipConfig, siteSettingId, BlockClientConfigType.ManualAdded);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
