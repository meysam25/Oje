using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FileService.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            FileAccessRoles = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public long Value { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("Role")]
        public List<FileAccessRole> FileAccessRoles { get; set; }
    }
}
