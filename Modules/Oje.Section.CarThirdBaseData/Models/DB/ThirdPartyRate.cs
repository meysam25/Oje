using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("ThirdPartyRates")]
    public class ThirdPartyRate
    {
        public ThirdPartyRate()
        {
            ThirdPartyRateCompanies = new List<ThirdPartyRateCompany>();
        }

        [Key]
        public int Id { get; set; }
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId")]
        [InverseProperty("ThirdPartyRates")]
        public CarSpecification CarSpecification { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int? Year { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("ThirdPartyRate")]
        public List<ThirdPartyRateCompany> ThirdPartyRateCompanies { get; set; }

    }
}
