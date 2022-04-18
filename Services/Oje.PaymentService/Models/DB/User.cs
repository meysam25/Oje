using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            BankAccounts = new();
            WalletTransactions = new();
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
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public long? ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("User")]
        public List<BankAccount> BankAccounts { get; set; }
        [InverseProperty("User")]
        public List<WalletTransaction> WalletTransactions { get; set; }
    }
}
