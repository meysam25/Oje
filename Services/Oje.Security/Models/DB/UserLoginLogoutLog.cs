using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("UserLoginLogoutLogs")]
    public class UserLoginLogoutLog
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserLoginLogoutLogs")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public UserLoginLogoutLogType Type { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Required, MaxLength(1000)]
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int SiteSettingId { get; set; }
    }
}
