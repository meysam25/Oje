using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            Roles = new List<Role>();
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
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("SiteSettings")]
        public User User { get; set; }
        public bool IsHttps { get; set; }
        [MaxLength(4000)]
        public string SeoMainPage { get; set; }
        [MaxLength(200)]
        public string Image96 { get; set; }
        [MaxLength(200)]
        public string Image192 { get; set; }
        [MaxLength(200)]
        public string Image512 { get; set; }
        [MaxLength(200)]
        public string ImageText { get; set; }

        [InverseProperty("SiteSetting")]
        public List<Role> Roles { get; set; }
    }
}
