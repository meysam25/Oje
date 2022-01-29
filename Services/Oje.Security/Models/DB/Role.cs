using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            FileAccessRoles = new List<FileAccessRole>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [InverseProperty("Role")]
        public List<FileAccessRole> FileAccessRoles { get; set; }
    }
}
