using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormJsons")]
    public class UserFilledRegisterFormJson: SignatureEntity
    {
        [Key]
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormJsons")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        [Required]
        public string JsonConfig { get; set; }
    }
}
