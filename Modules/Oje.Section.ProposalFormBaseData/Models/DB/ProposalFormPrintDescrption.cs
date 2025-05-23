﻿using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("ProposalFormPrintDescrptions")]
    public class ProposalFormPrintDescrption: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("ProposalFormPrintDescrptions")]
        public ProposalForm ProposalForm { get; set; }
        public ProposalFormPrintDescrptionType Type { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ProposalFormPrintDescrptions")]
        public SiteSetting SiteSetting { get; set; }

    }
}
