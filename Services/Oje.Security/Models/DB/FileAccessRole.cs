using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("FileAccessRoles")]
    public class FileAccessRole
    {
        [Key]
        public int Id { get; set; }
        public FileType FileType { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("FileAccessRoles")]
        public Role Role { get; set; }
    }
}
