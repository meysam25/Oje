using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormValues")]
    public class ProposalFilledFormValue
    {
        public ProposalFilledFormValue()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormValues")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public int ProposalFilledFormKeyId { get; set; }
        [ForeignKey("ProposalFilledFormKeyId"), InverseProperty("ProposalFilledFormValues")]
        public ProposalFilledFormKey ProposalFilledFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }

    }
}
