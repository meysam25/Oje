using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarBodyBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            CarSpecificationAmountCompanies = new List<CarSpecificationAmountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<CarSpecificationAmountCompany> CarSpecificationAmountCompanies { get; set; }
    }
}
