using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormJsons")]
    public class GeneralFormJson
    {
        [Key]
        public long GeneralFilledFormId { get; set; }
        [ForeignKey("GeneralFilledFormId"), InverseProperty("GeneralFormJsons")]
        public GeneralFilledForm GeneralFilledForm { get; set; }
        public long GeneralFormCacheJsonId { get; set; }
        [ForeignKey("GeneralFormCacheJsonId"), InverseProperty("GeneralFormJsons")]
        public GeneralFormCacheJson GeneralFormCacheJson { get; set; }
    }
}
