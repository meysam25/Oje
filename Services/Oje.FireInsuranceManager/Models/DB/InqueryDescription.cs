using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("InqueryDescriptions")]
    public class InqueryDescription
    {
        public InqueryDescription()
        {
            InqueryDescriptionCompanies = new List<InqueryDescriptionCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InqueryDescriptions")]
        public ProposalForm ProposalForm { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("InqueryDescription")]
        public List<InqueryDescriptionCompany> InqueryDescriptionCompanies { get; set; }
    }
}
