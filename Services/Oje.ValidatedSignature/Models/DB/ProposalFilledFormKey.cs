using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalFilledFormKeys")]
    public class ProposalFilledFormKey: SignatureEntity
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
