using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserFilledRegisterForms")]
    public class UserFilledRegisterForm
    {
        public UserFilledRegisterForm()
        {
            UserFilledRegisterFormJsons = new();
            UserFilledRegisterFormValues = new();
            UserFilledRegisterFormCompanies = new();
        }

        [Key]
        public long Id { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserFilledRegisterForms")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public long? Price { get; set; }
        [MaxLength(50)]
        public string PaymentTraceCode { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserFilledRegisterForms")]
        public User User { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public bool? IsDone { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormJson> UserFilledRegisterFormJsons { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
        [InverseProperty("UserFilledRegisterForm")]
        public List<UserFilledRegisterFormCompany> UserFilledRegisterFormCompanies { get; set; }
    }
}
