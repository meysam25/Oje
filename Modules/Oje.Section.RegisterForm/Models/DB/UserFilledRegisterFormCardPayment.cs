using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterFormCardPayments")]
    public class UserFilledRegisterFormCardPayment
    {
        [Key]
        public long Id { get; set; }
        public long UserFilledRegisterFormId { get; set; }
        [ForeignKey("UserFilledRegisterFormId"), InverseProperty("UserFilledRegisterFormCardPayments")]
        public UserFilledRegisterForm UserFilledRegisterForm { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(20)]
        public string CardNo { get; set; }
        [Required, MaxLength(50)]
        public string RefferCode { get; set; }
        public long Price { get; set; }
        public DateTime PayDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserFilledRegisterFormCardPayments")]
        public User User { get; set; }
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        public int SiteSettingId { get; set; }
    }
}
