using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormCacheJsons")]
    public class ProposalFilledFormCacheJson: SignatureEntity
    {
        public ProposalFilledFormCacheJson()
        {
            ProposalFilledFormJsons = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        public int HashCode { get; set; }

        [InverseProperty("ProposalFilledFormCacheJson")]
        public List<ProposalFilledFormJson> ProposalFilledFormJsons { get; set; }
    }
}
