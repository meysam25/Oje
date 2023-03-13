using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("BankAccountFactors")]
    public class BankAccountFactor: SignatureEntity
    {
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId"), InverseProperty("BankAccountFactors")]
        public BankAccount BankAccount { get; set; }
        public BankAccountFactorType Type { get; set; }
        public long ObjectId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public long Price { get; set; }
        [MaxLength(300)]
        public string TargetLink { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? PayDate { get; set; }
        public long? UserId { get; set; }
        [MaxLength(50)]
        public string TraceCode { get; set; }
        public DateTime? LastTryDate { get; set; }
        public int? CountTry { get; set; }
        public string LastErrorMessage { get; set; }
        public BankAccountType? BankAccountType { get; set; }
        public long? KeyHash { get; set; }
        [MaxLength(100)]
        public string Token { get; set; }
        public int SiteSettingId { get; set; }
    }
}
