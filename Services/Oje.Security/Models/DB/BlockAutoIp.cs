using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.DB
{
    [Table("BlockAutoIps")]
    public class BlockAutoIp
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int CreateYear { get; set; }
        public int CreateMonth { get; set; }
        public int CreateDay { get; set; }
        public BlockClientConfigType BlockClientConfigType { get; set; }
        public BlockAutoIpAction BlockAutoIpAction { get; set; }
        public int SiteSettingId { get; set; }
    }
}
