using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            GeneralFormStatusRoles = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [InverseProperty("Role")]
        public List<GeneralFormStatusRole> GeneralFormStatusRoles { get; set; }
    }
}
