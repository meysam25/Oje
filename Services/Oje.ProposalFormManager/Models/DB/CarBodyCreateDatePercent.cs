using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarBodyCreateDatePercents")]
    public class CarBodyCreateDatePercent
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
