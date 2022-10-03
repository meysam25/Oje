using Oje.Infrastructure.Interfac;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("ContactUses")]
    public class ContactUs: IEntityWithSiteSettingId
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(100)]
        public string Fullname { get; set; }
        [Required,MaxLength(50)]
        public string Tell { get; set; }
        [Required,MaxLength(100)]
        public string Email { get; set; }
        [Required,MaxLength(1000)]
        public string Description { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ContactUses")]
        public SiteSetting SiteSetting { get; set; }
    }
}
