using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            UserFilledRegisterForms = new();
            UserFilledRegisterFormCardPayments = new();
            UserRegisterForms = new();
            UserRegisterFormCompanies = new();
            UserRegisterFormDiscountCodes = new();
            UserRegisterFormPrintDescrptions = new();
            UserRegisterFormRequiredDocuments = new();
            UserRegisterFormRequiredDocumentTypes = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserFilledRegisterFormCardPayment> UserFilledRegisterFormCardPayments { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterForm> UserRegisterForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormCompany> UserRegisterFormCompanies { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormDiscountCode> UserRegisterFormDiscountCodes { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormPrintDescrption> UserRegisterFormPrintDescrptions { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormRequiredDocument> UserRegisterFormRequiredDocuments { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormRequiredDocumentType> UserRegisterFormRequiredDocumentTypes { get; set; }
    }
}
