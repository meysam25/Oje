using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageLeftRightDesignItems")]
    public class PageLeftRightDesignItem: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        public long PageLeftRightDesignId { get; set; }
        [ForeignKey("PageLeftRightDesignId"), InverseProperty("PageLeftRightDesignItems")]
        public PageLeftRightDesign PageLeftRightDesign { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        [Required, MaxLength(200)]
        public string MainImage { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        [MaxLength(200)]
        public string ButtonLink { get; set; }
        [MaxLength(50)]
        public string ButtonTitle { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("PageLeftRightDesignItems")]
        public SiteSetting SiteSetting { get; set; }
    }
}
