using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("GlobalDiscounts")]
    public class GlobalDiscount
    {
        public GlobalDiscount()
        {
            GlobalDiscountCompanies = new List<GlobalDiscountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("GlobalDiscounts")]
        public ProposalForm ProposalForm { get; set; }
        [MaxLength(30)]
        public string Code { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long? Price { get; set; }
        public int? Percent { get; set; }
        public int MaxUse { get; set; }
        public long MaxPrice { get; set; }
        [MaxLength(10)]
        public string InqueryCode { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreatedGlobalDiscounts")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdatedGlobalDiscounts")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("GlobalDiscount")]
        public List<GlobalDiscountCompany> GlobalDiscountCompanies { get; set; }
    }
}
