﻿using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("GlobalInqueries")]
    public class GlobalInquery: SignatureEntity
    {
        public GlobalInquery()
        {
            ProposalFilledForms = new();
            GlobalInquiryItems = new();
        }

        [Key]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("GlobalInqueries")]
        public Company Company { get; set; }
        public DateTime CreateDate { get; set; }
        public long GlobalInputInqueryId { get; set; }
        [ForeignKey("GlobalInputInqueryId"), InverseProperty("GlobalInqueries")]
        public GlobalInputInquery GlobalInputInquery { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("GlobalInqueries")]
        public ProposalForm ProposalForm { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("GlobalInquery")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
        [InverseProperty("GlobalInquery")]
        public List<GlobalInquiryItem> GlobalInquiryItems { get; set; }
        

        [NotMapped]
        public bool deleteMe { get; set; }
        [NotMapped]
        public decimal maxDiscount { get; set; }
    }
}
