using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogTagBlogs")]
    public class BlogTagBlog
    {
        [Key, Column(Order = 1)]
        public long BlogTagId { get; set; }
        [ForeignKey("BlogTagId")]
        [InverseProperty("BlogTagBlogs")]
        public BlogTag BlogTag { get; set; }
        [Key, Column(Order = 2)]
        public long BlogId { get; set; }
        [ForeignKey("BlogId")]
        [InverseProperty("BlogTagBlogs")]
        public Blog Blog { get; set; }
    }
}
