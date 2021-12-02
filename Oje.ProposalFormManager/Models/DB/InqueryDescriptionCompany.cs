using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("InqueryDescriptionCompanies")]
    public class InqueryDescriptionCompany
    {
        [Key]
        public long Id { get; set; }
        public int InqueryDescriptionId { get; set; }
        [ForeignKey("InqueryDescriptionId")]
        [InverseProperty("InqueryDescriptionCompanies")]
        public InqueryDescription InqueryDescription { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("InqueryDescriptionCompanies")]
        public Company Company { get; set; }
    }
}
