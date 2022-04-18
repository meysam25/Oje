using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageLeftRightDesigns")]
    public class PageLeftRightDesign
    {
        public PageLeftRightDesign()
        {
            PageLeftRightDesignItems = new();
        }

        [Key]
        public long Id { get; set; }
        public long PageId { get; set; }
        [ForeignKey("PageId"), InverseProperty("PageLeftRightDesigns")]
        public Page Page { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("PageLeftRightDesign")]
        public List<PageLeftRightDesignItem> PageLeftRightDesignItems { get; set; }
    }
}
