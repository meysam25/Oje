using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("GlobalInquiryItems")]
    public class GlobalInquiryItem
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
        [ForeignKey("GlobalDiscountId"), InverseProperty("GlobalInquiryItems")]
        public GlobalDiscount GlobalDiscount { get; set; }
        public int? Order { get; set; }


        [NotMapped]
        public long? basePriceEC { get; set; }
    }
}
