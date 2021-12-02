using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalFilledFormJsons")]
    public class ProposalFilledFormJson
    {
        [Key]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormJsons")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        [Required]
        public string JsonConfig { get; set; }
    }
}
