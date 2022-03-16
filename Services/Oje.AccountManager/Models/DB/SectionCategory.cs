using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("SectionCategories")]
    public class SectionCategory
    {
        public SectionCategory()
        {
            SectionCategorySections = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(30)]
        public string Icon { get; set; }

        [InverseProperty("SectionCategory")]
        public List<SectionCategorySection> SectionCategorySections { get; set; }
    }
}
