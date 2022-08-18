using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabCarTypes")]
    public class SanabCarType
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId"), InverseProperty("SanabCarTypes")]
        public CarType CarType { get; set; }
        public int Code { get; set; }
    }
}
