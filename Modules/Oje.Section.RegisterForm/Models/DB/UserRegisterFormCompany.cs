using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormCompanies")]
    public class UserRegisterFormCompany: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("UserRegisterFormCompanies")]
        public Company Company { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormCompanies")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
