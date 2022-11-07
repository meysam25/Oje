using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormCacheJsons")]
    public class GeneralFormCacheJson
    {
        public GeneralFormCacheJson()
        {
            GeneralFormJsons = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        public int HashCode { get; set; }

        [InverseProperty("GeneralFormCacheJson")]
        public List<GeneralFormJson> GeneralFormJsons { get; set; }
    }
}
