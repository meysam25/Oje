using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormCompanies")]
    public class ProposalFilledFormCompany: SignatureEntity
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
        [MaxLength(200)]
        public string MainFileUrl { get; set; }
        public long? CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("CreateUserProposalFilledFormCompanies")]
        public User CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId"), InverseProperty("UpdateUserProposalFilledFormCompanies")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
