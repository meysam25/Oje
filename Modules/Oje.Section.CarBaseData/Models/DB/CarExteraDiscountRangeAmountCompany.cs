using Oje.AccountManager.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("CarExteraDiscountRangeAmountCompanies")]
    public class CarExteraDiscountRangeAmountCompany
    {
        [Key]
        public long Id { get; set; }
        public int CarExteraDiscountRangeAmountId { get; set; }
        [ForeignKey("CarExteraDiscountRangeAmountId")]
        [InverseProperty("CarExteraDiscountRangeAmountCompanies")]
        public CarExteraDiscountRangeAmount CarExteraDiscountRangeAmount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("CarExteraDiscountRangeAmountCompanies")]
        public Company Company { get; set; }

    }
}
