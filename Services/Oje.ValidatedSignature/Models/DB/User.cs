using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("Users")]
    public class User : SignatureEntity
    {
        public User()
        {
            Childs = new();
            CreatedUsers = new();
            UpdatedUsers = new();
            UserRoles = new();
            UploadedFiles = new();
            TenderFilledForms = new();
            UserFilledRegisterForms = new();
            WalletTransactions = new();
            BankAccounts = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public User Parent { get; set; }
        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }

        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public bool IsMobileConfirm { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool IsEmailConfirm { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(12)]
        public string PostalCode { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string UserPic { get; set; }
        public long? CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId")]
        [InverseProperty("CreatedUsers")]
        public User CreateByUser { get; set; }
        [InverseProperty("CreateByUser")]
        public List<User> CreatedUsers { get; set; }
        public long? UpdateByUserId { get; set; }
        [ForeignKey("UpdateByUserId")]
        [InverseProperty("UpdatedUsers")]
        public User UpdateByUser { get; set; }
        [InverseProperty("UpdateByUser")]
        public List<User> UpdatedUsers { get; set; }
        public long? AgentCode { get; set; }
        [MaxLength(100)]
        public string CompanyTitle { get; set; }
        [MaxLength(20)]
        public string BankAccount { get; set; }
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(50)]
        public string InsuranceECode { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountInvalidPass { get; set; }
        public DateTime? TemproryLockDate { get; set; }
        public decimal? MapLat { get; set; }
        public decimal? MapLon { get; set; }
        public byte? MapZoom { get; set; }
        public DateTime? HireDate { get; set; }
        public Gender? Gender { get; set; }
        [MaxLength(20)]
        public string ShenasnameNo { get; set; }
        public MarrageStatus? MarrageStatus { get; set; }
        public int? BankId { get; set; }
        [MaxLength(50)]
        public string LastSessionFileName { get; set; }
        [MaxLength(50)]
        public string RefferCode { get; set; }
        public PersonType? RealOrLegaPerson { get; set; }
        public DateTime? LicenceExpireDate { get; set; }
        public int? StartHour { get; set; }
        public int? EndHour { get; set; }
        public bool? WorkingHolyday { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SiteSettingUsers")]
        public SiteSetting SiteSetting { get; set; }
        public bool? CanSeeOtherSites { get; set; }

        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("CreateByUser")]
        public List<UploadedFile> UploadedFiles { get; set; }
        [InverseProperty("User")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
        [InverseProperty("User")]
        public List<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        [InverseProperty("User")]
        public List<WalletTransaction> WalletTransactions { get; set; }
        [InverseProperty("User")]
        public List<BankAccount> BankAccounts { get; set; }
        [InverseProperty("User")]
        public List<BankAccountFactor> BankAccountFactors { get; set; }

    }
}
