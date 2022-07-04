using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            UserAdminLogs = new();
            Errors = new();
            BlockAutoIps = new();
            UserLoginLogoutLogs = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        [InverseProperty("User")]
        public List<UserAdminLog> UserAdminLogs { get; set; }
        [InverseProperty("User")]
        public List<Error> Errors { get; set; }
        [InverseProperty("User")]
        public List<BlockAutoIp> BlockAutoIps { get; set; }
        [InverseProperty("User")]
        public List<UserLoginLogoutLog> UserLoginLogoutLogs { get; set; }
    }
}
