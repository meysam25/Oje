using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalFilledForms")]
    public class ProposalFilledForm
    {
        public ProposalFilledForm()
        {
            ProposalFilledFormValues = new ();
            ProposalFilledFormUsers = new();
            ProposalFilledFormCompanies = new();
            GlobalDiscountUseds = new();
            ProposalFilledFormJsons = new();
            ProposalFilledFormDocuments = new();
            ProposalFilledFormStatusLogs = new();
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
        public long? GlobalInqueryId { get; set; }
        [ForeignKey("GlobalInqueryId"), InverseProperty("ProposalFilledForms")]
        public GlobalInquery GlobalInquery { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? IssueDate { get; set; }
        [MaxLength(50)]
        public string InsuranceNumber { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? InsuranceEndDate { get; set; }
        public bool IsDelete { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormValue> ProposalFilledFormValues { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<GlobalDiscountUsed> GlobalDiscountUseds { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormJson> ProposalFilledFormJsons { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormDocument> ProposalFilledFormDocuments { get; set; }
        [InverseProperty("ProposalFilledForm")]
        public List<ProposalFilledFormStatusLog> ProposalFilledFormStatusLogs { get; set; }
    }
}
