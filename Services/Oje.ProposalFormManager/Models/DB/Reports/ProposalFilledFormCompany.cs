using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("ProposalFilledFormCompanies")]
    public class ProposalFilledFormCompany
    {
        [Key, Column(Order = 1)]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("ProposalFilledFormCompanies")]
        public Company Company { get; set; }
        [Key, Column(Order = 2)]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormCompanies")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public long Price { get; set; }
        public bool IsSelected { get; set; }
    }
}
