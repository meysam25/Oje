using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarBodyBaseData.Models.DB
{
    [Table("CarSpecifications")]
    public class CarSpecification
    {
        public CarSpecification()
        {
            CarSpecificationAmounts = new List<CarSpecificationAmount>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [InverseProperty("CarSpecification")]
        public List<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
    }
}
