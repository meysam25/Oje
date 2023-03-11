using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("TenderFilledFormPrices")]
    public class TenderFilledFormPrice : SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormPrices")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int CompanyId { get; set; }
        public int TenderProposalFormJsonConfigId { get; set; }
        public long UserId { get; set; }
        public long Price { get; set; }
        [Required, MaxLength(200)]
        public string FilledFileUrl { get; set; }
        public bool IsConfirm { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public bool? IsPublished { get; set; }
    }
}
