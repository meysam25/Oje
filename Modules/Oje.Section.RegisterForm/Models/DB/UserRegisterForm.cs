using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterForms")]
    public class UserRegisterForm
    {
        public UserRegisterForm()
        {
            UserRegisterFormRequiredDocumentTypes = new();
            UserFilledRegisterForms = new();
            UserRegisterFormPrices = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(300)]
        public string Title { get; set; }
        [Required, MaxLength(300)]
        public string Name { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        public long? PaymentUserId { get; set; }
        [ForeignKey("PaymentUserId"), InverseProperty("UserRegisterForms")]
        public User PaymentUser { get; set; }
        [Required, MaxLength(4000)]
        public string SeoDescription { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4000)]
        public string TermDescription { get; set; }
        [MaxLength(200)]
        public string RuleFile { get; set; }
        [MaxLength(200)]
        public string SecountFile { get; set; }
        [MaxLength(200)]
        public string AnotherFile { get; set; }
        public int SiteSettingId { get; set; }


        [InverseProperty("UserRegisterForm")]
        public List<UserRegisterFormRequiredDocumentType> UserRegisterFormRequiredDocumentTypes { get; set; }
        [InverseProperty("UserRegisterForm")]
        public List<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        [InverseProperty("UserRegisterForm")]
        public List<UserRegisterFormPrice> UserRegisterFormPrices { get; set; }
    }
}
