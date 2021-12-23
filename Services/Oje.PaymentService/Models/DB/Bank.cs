using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.DB
{
    [Table("Banks")]
    public class Bank
    {
        public Bank()
        {
            BankAccounts = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [MaxLength(100)]
        public string Pic { get; set; }

        [InverseProperty("Bank")]
        public List<BankAccount> BankAccounts { get; set; }
    }
}
