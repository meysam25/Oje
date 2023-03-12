using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UserFilledRegisterFormKeys")]
    public class UserFilledRegisterFormKey: SignatureEntity
    {
        public UserFilledRegisterFormKey()
        {
            UserFilledRegisterFormValues = new();
        }

        [Key]
        public long Id { get; set; }
        [Required,MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("UserFilledRegisterFormKey")]
        public List<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
    }
}
