using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormCompanies")]
    public class UserFilledRegisterFormCompany
    {
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormCompanies")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("UserFilledRegisterFormCompanies")]
        public Company Company { get; set; }
    }
}
