using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
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
