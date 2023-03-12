using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UserFilledRegisterFormCardPayments")]
    public class UserFilledRegisterFormCardPayment: SignatureEntity, IEntityWithSiteSettingId
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
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        public int SiteSettingId { get; set; }
    }
}
