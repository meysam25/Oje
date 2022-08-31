using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormKeys")]
    public class ProposalFilledFormKey
    {
        public ProposalFilledFormKey()
        {
            ProposalFilledFormValues = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("ProposalFilledFormKey")]
        public List<ProposalFilledFormValue> ProposalFilledFormValues { get; set; }
    }
}
