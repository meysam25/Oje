using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarBodyBaseData.Models.DB
{
    [Table("CarSpecificationAmounts")]
    public class CarSpecificationAmount
    {
        public CarSpecificationAmount()
        {
            CarSpecificationAmountCompanies = new List<CarSpecificationAmountCompany>();
        }

        [Key]
        public int Id { get; set; }
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId")]
        [InverseProperty("CarSpecificationAmounts")]
        public CarSpecification CarSpecification { get; set; }
        public long MinAmount { get; set; }
        public long MaxAmount { get; set; }
        public decimal? Rate { get; set; }
        public long? Amount { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarSpecificationAmount")]
        public List<CarSpecificationAmountCompany> CarSpecificationAmountCompanies { get; set; }

    }
}
