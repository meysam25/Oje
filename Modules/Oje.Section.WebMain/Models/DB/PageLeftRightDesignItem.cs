using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageLeftRightDesignItems")]
    public class PageLeftRightDesignItem
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
    }
}
