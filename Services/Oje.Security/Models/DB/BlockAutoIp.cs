using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("BlockAutoIps")]
    public class BlockAutoIp: IEntityWithSiteSettingId
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
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("BlockAutoIps")]
        public User User { get; set; }
        [Required, MaxLength(50)]
        public string RequestId { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BlockAutoIps")]
        public SiteSetting SiteSetting { get; set; }
    }
}
