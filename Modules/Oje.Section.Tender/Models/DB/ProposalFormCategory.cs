using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("ProposalFormCategories")]
    public class ProposalFormCategory
    {
        public ProposalFormCategory()
        {
            ProposalForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("ProposalFormCategory")]
        public List<ProposalForm> ProposalForms { get; set; }
    }
}
