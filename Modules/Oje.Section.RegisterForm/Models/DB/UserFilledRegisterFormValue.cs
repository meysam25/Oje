using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormValues")]
    public class UserFilledRegisterFormValue: SignatureEntity
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
