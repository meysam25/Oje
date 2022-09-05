using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("GoogleBackupArchives")]
    public class GoogleBackupArchive
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(200)]
        public string FileId { get; set; }
        public long FileSize { get; set; }
    }
}
