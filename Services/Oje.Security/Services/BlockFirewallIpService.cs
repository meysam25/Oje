using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Services.EContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Services
{
    public class BlockFirewallIpService : IBlockFirewallIpService
    {
        readonly SecurityDBContext db = null;
        static object lockObj = null;
        public BlockFirewallIpService(SecurityDBContext db)
        {
            this.db = db;
        }

        public void Create(IpSections ipSections, int? siteSettingId, BlockClientConfigType type)
        {
            if (ipSections != null && siteSettingId.ToIntReturnZiro() > 0)
            {
                if(lockObj == null)
                    lockObj = new object();
                lock(lockObj)
                {
                    if (!db.BlockFirewallIps.Any(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4))
                    {
                        db.Entry(new BlockFirewallIp()
                        {
                            BlockClientConfigType = type,
                            CreateDate = DateTime.Now,
                            Ip1 = ipSections.Ip1,
                            Ip2 = ipSections.Ip2,
                            Ip3 = ipSections.Ip3,
                            Ip4 = ipSections.Ip4,
                            SiteSettingId = siteSettingId.Value
                        }).State = EntityState.Added;
                        db.SaveChanges();
                    }
                }
            }
        }

        public async Task RegisterInFirewall()
        {
            var allBlockIps = await db.BlockFirewallIps.Where(t => t.IsRead == null).ToListAsync();
            foreach (var ip in allBlockIps)
            {
                BanIP("OjeFirTCP" + ip.Ip1 + "_" + ip.Ip2 + "_" + ip.Ip3 + "_" + ip.Ip4, ip.Ip1 + "." + ip.Ip2 + "." + ip.Ip3 + "." + ip.Ip4, "Any", "TCP");
                BanIP("OjeFirUDP" + ip.Ip1 + "_" + ip.Ip2 + "_" + ip.Ip3 + "_" + ip.Ip4, ip.Ip1 + "." + ip.Ip2 + "." + ip.Ip3 + "." + ip.Ip4, "Any", "UDP");
                ip.IsRead = true;
                db.SaveChanges();
            }
        }

        void BanIP(string RuleName, string IPAddress, string Port, string Protocol)
        {
            if (OperatingSystem.IsWindows())
            {
                if (!string.IsNullOrEmpty(RuleName) && !string.IsNullOrEmpty(IPAddress) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Protocol) && new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    using (Process RunCmd = new Process())
                    {
                        RunCmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        RunCmd.StartInfo.FileName = "cmd.exe";
                        RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"" + RuleName + "\" dir=in action=block remoteip=" + IPAddress + " remoteport=" + Port + " protocol=" + Protocol;
                        RunCmd.Start();
                    }
                }
            }
        }
    }
}
