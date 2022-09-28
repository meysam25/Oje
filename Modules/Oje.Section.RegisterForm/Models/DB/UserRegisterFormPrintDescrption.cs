using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormPrintDescrptions")]
    public class UserRegisterFormPrintDescrption: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormPrintDescrptions")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public ProposalFormPrintDescrptionType Type { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
