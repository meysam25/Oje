using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            CarExteraDiscountRangeAmountCompanies = new List<CarExteraDiscountRangeAmountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        public List<CarExteraDiscountRangeAmountCompany> CarExteraDiscountRangeAmountCompanies { get; set; }
    }
}
