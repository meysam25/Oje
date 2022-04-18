using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("WalletTransactions")]
    public class WalletTransaction
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("WalletTransactions")]
        public User User { get; set; }
        public long Price { get; set; }
        public string Descrption { get; set; }
        public int SiteSettingId { get; set; }
    }
}
