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
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("GeneralFilledFormKey")]
        public List<GeneralFilledFormValue> GeneralFilledFormValues { get; set; }
    }
}
