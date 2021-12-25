using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("ProposalFilledForms")]
    public class ProposalFilledForm
    {
        public ProposalFilledForm()
        {
        }

        [Key]
        public long Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("ProposalFilledForms")]
        public ProposalForm ProposalForm { get; set; }
        public long Price { get; set; }
        public ProposalFilledFormStatus Status { get; set; }
        [MaxLength(50)]
        public string PaymentTraceCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? InsuranceEndDate { get; set; }
        public bool IsDelete { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
    }
}
