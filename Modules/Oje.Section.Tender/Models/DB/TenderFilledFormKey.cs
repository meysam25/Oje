using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormKeys")]
    public class TenderFilledFormKey
    {
        public TenderFilledFormKey()
        {
            TenderFilledFormsValues = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("TenderFilledFormKey")]
        public List<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
    }
}
