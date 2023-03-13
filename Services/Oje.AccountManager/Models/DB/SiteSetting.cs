using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting: SignatureEntity
    {
        public SiteSetting()
        {
            Roles = new();
            Childs = new();
            ExternalNotificationServiceConfigs = new();
            UserNotifications = new();
            UserNotificationTemplates = new();
            UserNotificationTrigers = new();
            SiteSettingUsers = new();
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
        public int? ParentId { get; set; }
        [ForeignKey("ParentId "), InverseProperty("Childs")]
        public SiteSetting Parent { get; set; }
        public WebsiteType? WebsiteType { get; set; }
        [MaxLength(200)]
        public string Image512Invert { get; set; }
        [MaxLength(100)]
        public string CopyRightTitle { get; set; }

        [InverseProperty("Parent")]
        public List<SiteSetting> Childs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<Role> Roles { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ExternalNotificationServiceConfig> ExternalNotificationServiceConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserNotification> UserNotifications { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserNotificationTemplate> UserNotificationTemplates { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserNotificationTriger> UserNotificationTrigers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<User> SiteSettingUsers { get; set; }
    }
}
