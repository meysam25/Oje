using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("GlobalInquiryItems")]
    public class GlobalInquiryItem: SignatureEntity
    {

        public GlobalInquiryItem()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public long GlobalInquiryId { get; set; }
        [ForeignKey("GlobalInquiryId"), InverseProperty("GlobalInquiryItems")]
        public GlobalInquery GlobalInquery { get; set; }
        [Required,MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        [Required,MaxLength(10)]
        public string CalcKey { get; set; }
        public bool Expired { get; set; }
        public int? GlobalDiscountId { get; set; }
        public int? Order { get; set; }

    }
}
