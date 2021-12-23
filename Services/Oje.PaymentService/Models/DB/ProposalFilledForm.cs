using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("ProposalFilledForms")]
    public class ProposalFilledForm
    {
        public ProposalFilledForm()
        {
        }

        [Key]
        public long Id { get; set; }
        public long Price { get; set; }
        [MaxLength(50)]
        public string PaymentTraceCode { get; set; }
        public int SiteSettingId { get; set; }

    }
}
