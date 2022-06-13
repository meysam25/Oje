using Oje.Infrastructure.Interfac;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormPrices")]
    public class TenderFilledFormPrice : IEntityWithUserId<User, long>
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormPrices")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("TenderFilledFormPrices")]
        public Company Company { get; set; }
        public int TenderProposalFormJsonConfigId { get; set; }
        [ForeignKey("TenderProposalFormJsonConfigId"), InverseProperty("TenderFilledFormPrices")]
        public TenderProposalFormJsonConfig TenderProposalFormJsonConfig { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("TenderFilledFormPrices")]
        public User User { get; set; }
        public long Price { get; set; }
        [Required, MaxLength(200)]
        public string FilledFileUrl { get; set; }
        public bool IsConfirm { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public bool? IsPublished { get; set; }
    }
}
