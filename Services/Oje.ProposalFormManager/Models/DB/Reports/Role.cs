using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("Roles")]
    public  class Role
    {
        public Role()
        {
            UserRoles = new ();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
    }
}
