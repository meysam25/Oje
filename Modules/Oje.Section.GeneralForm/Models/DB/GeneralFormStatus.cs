using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormStatuses")]
    public class GeneralFormStatus
    {
        public GeneralFormStatus()
        {
            GeneralFilledForms = new();
        }

        [Key]
        public long Id { get; set; }
        public long GeneralFormId { get; set; }
        [ForeignKey("GeneralFormId"), InverseProperty("GeneralFormStatuses")]
        public GeneralForm GeneralForm { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int Order { get; set; }

        [InverseProperty("GeneralFormStatus")]
        public List<GeneralFilledForm> GeneralFilledForms { get; set; }
    }
}
