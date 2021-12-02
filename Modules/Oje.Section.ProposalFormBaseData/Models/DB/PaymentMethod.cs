using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("PaymentMethods")]
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            PaymentMethodCompanies = new List<PaymentMethodCompany>();
            PaymentMethodFiles = new List<PaymentMethodFile>();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }
        public PaymentMethodType Type { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("PaymentMethods")]
        public ProposalForm ProposalForm { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsChek { get; set; }
        public int? PrePayPercent { get; set; }
        public int? DebitCount { get; set; }
        public int SiteSettingId { get; set; }


        [InverseProperty("PaymentMethod")]
        public List<PaymentMethodCompany> PaymentMethodCompanies { get; set; }
        [InverseProperty("PaymentMethod")]
        public List<PaymentMethodFile> PaymentMethodFiles { get; set; }

    }
}
