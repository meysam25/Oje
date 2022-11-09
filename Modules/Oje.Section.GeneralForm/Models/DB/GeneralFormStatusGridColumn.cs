using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormStatusGridColumns")]
    public class GeneralFormStatusGridColumn
    {
        [Key]
        public int Id { get; set; }
        public long GeneralFormId { get; set; }
        [ForeignKey("GeneralFormId"), InverseProperty("GeneralFormStatusGridColumns")]
        public GeneralForm GeneralForm { get; set; }
        public long GeneralFormStatusId { get; set; }
        [ForeignKey("GeneralFormStatusId"), InverseProperty("GeneralFormStatusGridColumns")]
        public GeneralFormStatus GeneralFormStatus { get; set; }
        public int GeneralFilledFormKeyId { get; set; }
        [ForeignKey("GeneralFilledFormKeyId"), InverseProperty("GeneralFormStatusGridColumns")]
        public GeneralFilledFormKey GeneralFilledFormKey { get; set; }
        public int Order { get; set; }
    }
}
