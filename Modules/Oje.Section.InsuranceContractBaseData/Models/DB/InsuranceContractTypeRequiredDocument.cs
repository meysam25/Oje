using Oje.Infrastructure.Interfac;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractTypeRequiredDocuments")]
    public class InsuranceContractTypeRequiredDocument: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractTypeRequiredDocuments")]
        public InsuranceContractType InsuranceContractType { get; set; }
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId"), InverseProperty("InsuranceContractTypeRequiredDocuments")]
        public InsuranceContract InsuranceContract { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        [MaxLength(100)]
        public string DownloadFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractTypeRequiredDocuments")]
        public SiteSetting SiteSetting { get; set; }
    }
}
