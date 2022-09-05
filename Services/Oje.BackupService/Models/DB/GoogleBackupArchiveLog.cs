using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.BackupService.Models.DB
{
    [Table("GoogleBackupArchiveLogs")]
    public class GoogleBackupArchiveLog
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(200)]
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public GoogleBackupArchiveLogType Type { get; set; }
    }
}
