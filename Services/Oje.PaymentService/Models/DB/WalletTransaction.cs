using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("WalletTransactions")]
    public class WalletTransaction: SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("WalletTransactions")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public long Price { get; set; }
        public string Descrption { get; set; }
        [MaxLength(50)]
        public string TraceNo { get; set; }
        public int SiteSettingId { get; set; }
    }
}
