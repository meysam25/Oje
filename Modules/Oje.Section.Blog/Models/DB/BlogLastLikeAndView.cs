using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogLastLikeAndViews")]
    public class BlogLastLikeAndView
    {
        [Key, Column(Order = 1)]
        public byte Ip1 { get; set; }
        [Key, Column(Order = 2)]
        public byte Ip2 { get; set; }
        [Key, Column(Order = 3)]
        public byte Ip3 { get; set; }
        [Key, Column(Order = 4)]
        public byte Ip4 { get; set; }
        [Key, Column(Order = 5)]
        public BlogLastLikeAndViewType Type { get; set; }
        [Key, Column(Order = 6)]
        public long BlogId { get; set; }
        [ForeignKey("BlogId")]
        [InverseProperty("BlogLastLikeAndViews")]
        public Blog Blog { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
