using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        [Key]
        public long Id { get; set; }
        public long? ObjectId { get; set; }
        public FileType FileType { get; set; }
        public int? SiteSettingId { get; set; }
    }
}
