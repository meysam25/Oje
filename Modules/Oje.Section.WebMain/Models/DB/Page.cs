using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("Pages")]
    public class Page
    {
        public Page()
        {
            PageLeftRightDesigns = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string SubTitle { get; set; }
        [MaxLength(20)]
        public string TitleAndSubtitleColorCode { get; set; }
        [Required, MaxLength(1000)]
        public string Summery { get; set; }
        [Required, MaxLength(200)]
        public string MainImage { get; set; }
        [Required, MaxLength(200)]
        public string MainImageSmall { get; set; }
        [MaxLength(50)]
        public string ButtonTitle { get; set; }
        [MaxLength(200)]
        public string ButtonLink { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("Page")]
        public List<PageLeftRightDesign> PageLeftRightDesigns { get; set; }
    }
}
