using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        [Key]
        public long Id { get; set; }
        public FileType FileType { get; set; }
    }
}
