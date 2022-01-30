using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.DB
{
    [Table("BlockFirewallIps")]
    public class BlockFirewallIp
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public DateTime CreateDate { get; set; }
        public BlockClientConfigType BlockClientConfigType { get; set; }
        public bool? IsRead { get; set; }
        public int SiteSettingId { get; set; }
    }
}
