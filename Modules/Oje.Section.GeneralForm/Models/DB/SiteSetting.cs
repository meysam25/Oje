using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            GeneralForms = new();
            GeneralFilledForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<GeneralForm> GeneralForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<GeneralFilledForm> GeneralFilledForms { get; set; }
    }
}
