using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFilledFormValues")]
    public class GeneralFilledFormValue
    {
        [Key]
        public Guid Id { get; set; }
        public long GeneralFilledFormId { get; set; }
        [ForeignKey("GeneralFilledFormId"), InverseProperty("GeneralFilledFormValues")]
        public GeneralFilledForm GeneralFilledForm { get; set; }
        public int GeneralFilledFormKeyId { get; set; }
        [ForeignKey("GeneralFilledFormKeyId"), InverseProperty("GeneralFilledFormValues")]
        public GeneralFilledFormKey GeneralFilledFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }

    }
}
