using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogCategories")]
    public class BlogCategory: IEntityWithSiteSettingId
    {
        public BlogCategory()
        {
            Blogs = new List<Blog>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BlogCategories")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("BlogCategory")]
        public List<Blog> Blogs { get; set; }
    }
}
