using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogTags")]
    public class BlogTag
    {
        public BlogTag()
        {
            BlogTagBlogs = new List<BlogTagBlog>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("BlogTag")]
        public List<BlogTagBlog> BlogTagBlogs { get; set; }
    }
}
