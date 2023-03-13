using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("SiteSettings")]
    public  class SiteSetting: SignatureEntity
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
            BankAccounts = new();
            BankAccountSadads = new();
            BankAccountSeps = new();
            BankAccountSizpaies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string WebsiteUrl { get; set; }
        [Required]
        [MaxLength(100)]
        public string PanelUrl { get; set; }
        public long UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        public bool IsHttps { get; set; }
        [MaxLength(4000)]
        public string SeoMainPage { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(200)]
        public string Image96 { get; set; }
        [MaxLength(200)]
        public string Image192 { get; set; }
        [MaxLength(200)]
        public string Image512 { get; set; }
        [MaxLength(200)]
        public string ImageText { get; set; }
        public int? ParentId { get; set; }
        public WebsiteType? WebsiteType { get; set; }
        [MaxLength(200)]
        public string Image512Invert { get; set; }
        [MaxLength(100)]
        public string CopyRightTitle { get; set; }

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
        [InverseProperty("SiteSetting")]
        public List<BankAccount> BankAccounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountFactor> BankAccountFactors { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSadad> BankAccountSadads { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSep> BankAccountSeps { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSizpay> BankAccountSizpaies { get; set; }
    }
}
