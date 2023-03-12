using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("SiteSettings")]
    public  class SiteSetting
    {
        public SiteSetting()
        {
            ProposalFilledForms = new();
            UploadedFiles = new();
            UserFilledRegisterForms = new();
            Roles = new();
            SiteSettingUsers = new();
            SmsValidationHistories = new();
            WalletTransactions = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UploadedFile> UploadedFiles { get; set; }
        [InverseProperty("SiteSetting")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<Role> Roles { get; set; }
        [InverseProperty("SiteSetting")]
        public List<User> SiteSettingUsers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<SmsValidationHistory> SmsValidationHistories { get; set; }
        [InverseProperty("SiteSetting")]
        public List<WalletTransaction> WalletTransactions { get; set; }
    }
}
