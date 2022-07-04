using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("Sections")]
    public class Section
    {
        public Section()
        {
            Controllers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [InverseProperty("Section")]
        public List<Controller> Controllers { get; set; }
    }
}
