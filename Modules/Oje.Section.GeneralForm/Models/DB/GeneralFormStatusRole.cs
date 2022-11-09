using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormStatusRoles")]
    public class GeneralFormStatusRole
    {
        [Key]
        public int Id { get; set; }
        public long GeneralFormId { get; set; }
        [ForeignKey("GeneralFormId"), InverseProperty("GeneralFormStatusRoles")]
        public GeneralForm GeneralForm { get; set; }
        public long? GeneralFormStatusId { get; set; }
        [ForeignKey("GeneralFormStatusId"), InverseProperty("GeneralFormStatusRoles")]
        public GeneralFormStatus GeneralFormStatus { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("GeneralFormStatusRoles")]
        public Role Role { get; set; }
    }
}
