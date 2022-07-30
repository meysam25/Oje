using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormDiscountCodeUses")]
    public class UserRegisterFormDiscountCodeUse
    {
        [Key]
        public long Id { get; set; }
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserRegisterFormDiscountCodeUses")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public int UserRegisterFormDiscountCodeId { get; set; }
        [ForeignKey("UserRegisterFormDiscountCodeId"), InverseProperty("UserRegisterFormDiscountCodeUses")]
        public UserRegisterFormDiscountCode UserRegisterFormDiscountCode { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserRegisterFormDiscountCodeUses")]
        public User User { get; set; }
    }
}
