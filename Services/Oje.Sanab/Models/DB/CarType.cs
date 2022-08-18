using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("CarTypes")]
    public class CarType
    {
        public CarType()
        {
            SanabCarTypes = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }

        [InverseProperty("CarType")]
        public List<SanabCarType> SanabCarTypes { get; set; }
    }
}
