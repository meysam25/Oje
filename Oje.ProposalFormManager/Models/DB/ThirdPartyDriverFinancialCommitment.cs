using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ThirdPartyDriverFinancialCommitments")]
    public class ThirdPartyDriverFinancialCommitment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}
