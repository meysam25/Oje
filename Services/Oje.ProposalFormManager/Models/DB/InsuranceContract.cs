using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InsuranceContracts")]
    public class InsuranceContract
    {
        public InsuranceContract()
        {
            InsuranceContractDiscounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InsuranceContracts")]
        public ProposalForm ProposalForm { get; set; }
        public long CreateUserId { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }

    }
}
