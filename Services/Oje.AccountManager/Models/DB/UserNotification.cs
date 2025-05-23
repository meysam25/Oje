﻿using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserNotifications")]
    public class UserNotification : IEntityWithUserId<User, long>, IEntityWithSiteSettingId
    {
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserNotifications")]
        public User User { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public UserNotificationType Type { get; set; }
        public long? FromUserId { get; set; }
        [ForeignKey("FromUserId"), InverseProperty("FromUserUserNotifications")]
        public User FromUser { get; set; }
        public long? ObjectId { get; set; }
        [Required, MaxLength(200)]
        public string Subject { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string TargetPageLink { get; set; }
        public DateTime? ViewDate { get; set; }
        public DateTime? LastTryDate { get; set; }
        public bool? IsSuccess { get; set; }
        public int? CountTry { get; set; }
        public bool? IsModal { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("UserNotifications")]
        public SiteSetting SiteSetting { get; set; }
    }
}
