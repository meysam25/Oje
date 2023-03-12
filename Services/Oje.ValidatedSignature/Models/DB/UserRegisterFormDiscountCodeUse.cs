using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UserRegisterFormDiscountCodeUses")]
    public class UserRegisterFormDiscountCodeUse: SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserRegisterFormDiscountCodeUses")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public int UserRegisterFormDiscountCodeId { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
    }
}
