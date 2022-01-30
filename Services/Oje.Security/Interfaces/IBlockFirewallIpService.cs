using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Interfaces
{
    public interface IBlockFirewallIpService
    {
        void Create(IpSections ipSections, int? siteSettingId, BlockClientConfigType type);
        Task RegisterInFirewall();
    }
}
