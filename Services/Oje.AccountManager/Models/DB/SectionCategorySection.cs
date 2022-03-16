using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("SectionCategorySections")]
    public class SectionCategorySection
    {
        public int SectionCategoryId { get; set; }
        [ForeignKey("SectionCategoryId")]
        [InverseProperty("SectionCategorySections")]
        public SectionCategory SectionCategory { get; set; }
        public int SectionId { get; set;}
        [ForeignKey("SectionId"), InverseProperty("SectionCategorySections")]
        public Section Section { get; set; }
    }
}
