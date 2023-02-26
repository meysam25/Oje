using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("Sections")]
    public class Section
    {
        public Section()
        {
            Controllers = new ();
            SectionCategorySections = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }
        public int? Order { get; set; }

        [InverseProperty("Section")]
        public List<Controller> Controllers { get; set; }
        [InverseProperty("Section")]
        public List<SectionCategorySection> SectionCategorySections { get; set; }
    }
}
