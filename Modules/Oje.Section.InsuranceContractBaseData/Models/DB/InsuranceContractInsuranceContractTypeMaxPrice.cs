﻿using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractInsuranceContractTypeMaxPrices")]
    public class InsuranceContractInsuranceContractTypeMaxPrice: IEntityWithSiteSettingId
    {
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId"), InverseProperty("InsuranceContractInsuranceContractTypeMaxPrices")]
        public InsuranceContract InsuranceContract { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractInsuranceContractTypeMaxPrices")]
        public InsuranceContractType InsuranceContractType { get; set; }
        public long MaxPrice { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractInsuranceContractTypeMaxPrices")]
        public SiteSetting SiteSetting { get; set; }
    }
}
