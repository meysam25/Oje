using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("Controllers")]
    public class Controller
    {
        public Controller()
        {
            Actions = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int SectionId { get; set; }
        [ForeignKey("SectionId"), InverseProperty("Controllers")]
        public Section Section { get; set; }

        [InverseProperty("Controller")]
        public List<Action> Actions { get; set; }
    }
}
