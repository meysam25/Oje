using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormJsons")]
    public class ProposalFilledFormJson
    {
        [Key]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormJsons")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public long ProposalFilledFormCacheJsonId { get; set; }
        [ForeignKey("ProposalFilledFormCacheJsonId"), InverseProperty("ProposalFilledFormJsons")]
        public ProposalFilledFormCacheJson ProposalFilledFormCacheJson { get; set; }
    }
}
