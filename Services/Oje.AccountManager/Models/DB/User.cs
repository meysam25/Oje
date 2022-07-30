using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("Users")]
    public class User : IEntityWithId<User, long>, EntityWithParent<User>
    {
        public User()
        {
            Childs = new();
            CreatedUsers = new();
            UpdatedUsers = new();
            UserRoles = new();
            UserCompanies = new();
            UserNotifications = new();
            FromUserUserNotifications = new();
            UserNotificationTrigers = new();
            SiteSettings = new();
            ExternalNotificationServicePushSubscriptions = new();
            WalletTransactions = new();
            FromUserUserMessages = new();
            ToUserUserMessages = new();
            UserMessageReplies = new();
            UserAdminLogs = new();
            UserRequestActions = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public User Parent { get; set; }
        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }

        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public bool IsMobileConfirm { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool IsEmailConfirm { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(12)]
        public string PostalCode { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string UserPic { get; set; }
        public long? CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId")]
        [InverseProperty("CreatedUsers")]
        public User CreateByUser { get; set; }
        [InverseProperty("CreateByUser")]
        public List<User> CreatedUsers { get; set; }
        public long? UpdateByUserId { get; set; }
        [ForeignKey("UpdateByUserId")]
        [InverseProperty("UpdatedUsers")]
        public User UpdateByUser { get; set; }
        [InverseProperty("UpdateByUser")]
        public List<User> UpdatedUsers { get; set; }
        public long? AgentCode { get; set; }
        [MaxLength(100)]
        public string CompanyTitle { get; set; }
        [MaxLength(20)]
        public string BankAccount { get; set; }
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(50)]
        public string InsuranceECode { get; set; }
        public int? ProvinceId { get; set; }
        [ForeignKey("ProvinceId"), InverseProperty("Users")]
        public Province Province { get; set; }
        public int? CityId { get; set; }
        [ForeignKey("CityId"), InverseProperty("Users")]
        public City City { get; set; }
        public int? CountInvalidPass { get; set; }
        public DateTime? TemproryLockDate { get; set; }
        public decimal? MapLat { get; set; }
        public decimal? MapLon { get; set; }
        public byte? MapZoom { get; set; }
        public NetTopologySuite.Geometries.Point MapLocation { get; set; }
        public DateTime? HireDate { get; set; }
        public Gender? Gender { get; set; }
        [MaxLength(20)]
        public string ShenasnameNo { get; set; }
        public MarrageStatus? MarrageStatus { get; set; }
        public int? BankId { get; set; }
        [MaxLength(50)]
        public string LastSessionFileName { get; set; }
        [MaxLength(50)]
        public string RefferCode { get; set; }
        public PersonType? RealOrLegaPerson { get; set; }
        public DateTime? LicenceExpireDate { get; set; }
        public int? SiteSettingId { get; set; }

        [NotMapped]
        public double distance { get; set; }


        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("User")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("User")]
        public List<UserNotification> UserNotifications { get; set; }
        [InverseProperty("FromUser")]
        public List<UserNotification> FromUserUserNotifications { get; set; }
        [InverseProperty("User")]
        public List<UserNotificationTriger> UserNotificationTrigers { get; set; }
        [InverseProperty("User")]
        public List<SiteSetting> SiteSettings { get; set; }
        [InverseProperty("User")]
        public List<ExternalNotificationServicePushSubscription> ExternalNotificationServicePushSubscriptions { get; set; }
        [InverseProperty("User")]
        public List<WalletTransaction> WalletTransactions { get; set; }
        [InverseProperty("FromUser")]
        public List<UserMessage> FromUserUserMessages { get; set; }
        [InverseProperty("ToUser")]
        public List<UserMessage> ToUserUserMessages { get; set; }
        [InverseProperty("User")]
        public List<UserMessageReply> UserMessageReplies { get; set; }
        [InverseProperty("User")]
        public List<UserAdminLog> UserAdminLogs { get; set; }
        [InverseProperty("User")]
        public List<UserRequestAction> UserRequestActions { get; set; }

    }
}
