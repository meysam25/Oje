using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledForms")]
    public class InsuranceContractProposalFilledForm: IEntityWithSiteSettingId
    {
        public InsuranceContractProposalFilledForm()
        {
            InsuranceContractProposalFilledFormValues = new();
            InsuranceContractProposalFilledFormJsons = new();
            InsuranceContractProposalFilledFormUsers = new();
        }

        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("InsuranceContractProposalFilledForms")]
        public User CreateUser { get; set; }
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId"), InverseProperty("InsuranceContractProposalFilledForms")]
        public InsuranceContract InsuranceContract { get; set; }
        public ContractLocation? ReciveLocation { get; set; }
        public DateTime? ReciveDate { get; set; }
        [MaxLength(20)]
        public string ReciveTime { get; set; }
        [MaxLength(50)]
        public string ReciveTell { get; set; }
        [MaxLength(4000)]
        public string ReciveAddress { get; set; }
        public byte? ReciveZoom { get; set; }
        public NetTopologySuite.Geometries.Point ReciveMapLocation { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractProposalFilledForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractProposalFilledForm")]
        public List<InsuranceContractProposalFilledFormValue> InsuranceContractProposalFilledFormValues { get; set; }
        [InverseProperty("InsuranceContractProposalFilledForm")]
        public List<InsuranceContractProposalFilledFormJson> InsuranceContractProposalFilledFormJsons { get; set; }
        [InverseProperty("InsuranceContractProposalFilledForm")]
        public List<InsuranceContractProposalFilledFormUser> InsuranceContractProposalFilledFormUsers { get; set; }
    }
}
