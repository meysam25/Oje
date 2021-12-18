using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            SmsTrigers = new();
            UserRoles = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [InverseProperty("Role")]
        public List<SmsTriger> SmsTrigers { get; set; }
        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
    }
}
