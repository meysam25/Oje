using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("DebugEmails")]
    public class DebugEmail
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(200)]
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        [Required, MaxLength(100)]
        public string SmtpHost { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Timeout { get; set; }
        public bool IsActive { get; set; }
    }
}
