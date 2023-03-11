using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormCompanies")]
    public class UserFilledRegisterFormCompany: SignatureEntity
    {
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormCompanies")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("UserFilledRegisterFormCompanies")]
        public Company Company { get; set; }
    }
}
