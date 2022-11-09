using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFilledFormKeys")]
    public class GeneralFilledFormKey
    {
        public GeneralFilledFormKey()
        {
            GeneralFilledFormValues = new();
            GeneralFormStatusGridColumns = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Key { get; set; }
        [Required, MaxLength(1000)]
        public string Title { get; set; }

        [InverseProperty("GeneralFilledFormKey")]
        public List<GeneralFilledFormValue> GeneralFilledFormValues { get; set; }
        [InverseProperty("GeneralFilledFormKey")]
        public List<GeneralFormStatusGridColumn> GeneralFormStatusGridColumns { get; set; }
    }
}
