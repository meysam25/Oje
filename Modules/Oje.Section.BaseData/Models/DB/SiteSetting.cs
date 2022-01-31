using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string WebsiteUrl { get; set; }
        [Required]
        [MaxLength(100)]
        public string PanelUrl { get; set; }
        public long UserId { get; set; }
        [InverseProperty("SiteSettings")]
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserSiteSettings")]
        public User CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserSiteSettings")]
        public User UpdateUser { get; set; }
        public bool IsHttps { get; set; }
        [MaxLength(4000)]
        public string SeoMainPage { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(200)]
        public string Image96 { get; set; }
        [MaxLength(200)]
        public string Image192 { get; set; }
        [MaxLength(200)]
        public string Image512 { get; set; }
    }
}
