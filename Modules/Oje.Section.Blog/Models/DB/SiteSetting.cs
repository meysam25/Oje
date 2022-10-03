using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Blog.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            Blogs = new();
            BlogCategories = new();
            BlogReviews = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<Blog> Blogs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BlogCategory> BlogCategories { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BlogReview> BlogReviews { get; set; }
    }
}
