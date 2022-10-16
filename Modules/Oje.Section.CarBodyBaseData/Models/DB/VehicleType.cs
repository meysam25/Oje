using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarBodyBaseData.Models.DB
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        public VehicleType()
        {
            CarBodyCreateDatePercents = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [InverseProperty("VehicleType")]
        public List<CarBodyCreateDatePercent> CarBodyCreateDatePercents { get; set; }
    }
}
