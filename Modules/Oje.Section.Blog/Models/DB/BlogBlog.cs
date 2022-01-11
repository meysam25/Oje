using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogBlogs")]
    public class BlogBlog
    {
        [Key, Column(Order = 1)]
        public long BlogOwnId { get; set; }
        [ForeignKey("BlogOwnId")]
        [InverseProperty("BlogOwnBlogs")]
        public Blog BlogOwn { get; set; }
        [Key, Column(Order = 2)]
        public long BlogRelatedId { get; set; }
        [ForeignKey("BlogRelatedId")]
        [InverseProperty("BlogRelatedBlogs")]
        public Blog BlogRelated { get; set; }

    }
}
