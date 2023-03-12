using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UserFilledRegisterForms")]
    public class UserFilledRegisterForm : SignatureEntity, IEntityWithSiteSettingId
    {
        public UserFilledRegisterForm()
        {
            UserFilledRegisterFormJsons = new();
            UserFilledRegisterFormValues = new();
            UserFilledRegisterFormCompanies = new();
            UserFilledRegisterFormCardPayments = new();
            UserRegisterFormDiscountCodeUses = new();
        }

        [Key]
        public long Id { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserRegisterFormId { get; set; }
        public long? Price { get; set; }
        [MaxLength(50)]
        public string PaymentTraceCode { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserFilledRegisterForms")]
        public User User { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public bool? IsDone { get; set; }
        [MaxLength(50)]
        public string RefferCode { get; set; }
        public long? RefferUserId { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("UserFilledRegisterForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormJson> UserFilledRegisterFormJsons { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormCompany> UserFilledRegisterFormCompanies { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormCardPayment> UserFilledRegisterFormCardPayments { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserRegisterFormDiscountCodeUse> UserRegisterFormDiscountCodeUses { get; set; }
    }
}
