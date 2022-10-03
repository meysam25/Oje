using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("LoginDescrptions")]
    public class LoginDescrption: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("LoginDescrptions")]
        public SiteSetting SiteSetting { get; set; }
    }
}
