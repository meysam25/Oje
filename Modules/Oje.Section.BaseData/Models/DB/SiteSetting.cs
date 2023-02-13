using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            Childs = new();
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
        [MaxLength(200)]
        public string ImageText { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId "), InverseProperty("Childs")]
        public SiteSetting Parent { get; set; }
        public WebsiteType? WebsiteType { get; set; }
        [MaxLength(200)]
        public string Image512Invert { get; set; }

        [InverseProperty("Parent")]
        public List<SiteSetting> Childs { get; set; }
    }
}
