using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormValues")]
    public class UserFilledRegisterFormValue
    {
        [Key]
        public Guid Id { get; set; }
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormValues")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public long UserFilledRegisterFormKeyId { get; set; }
        [ForeignKey("UserFilledRegisterFormKeyId"), InverseProperty("UserFilledRegisterFormValues")]
        public UserFilledRegisterFormKey UserFilledRegisterFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }
    }
}
