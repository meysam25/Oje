using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.DB
{
    [Table("BlogReviews")]
    public class BlogReview
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public bool IsConfirm { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public long BlogId { get; set; }
        [ForeignKey("BlogId")]
        [InverseProperty("BlogReviews")]
        public Blog Blog { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public long? ConfirmUserId { get; set; }
        [ForeignKey("ConfirmUserId")]
        [InverseProperty("BlogReviews")]
        public User ConfirmUser { get; set; }
        public int SiteSettingId { get; set; }
    }
}
