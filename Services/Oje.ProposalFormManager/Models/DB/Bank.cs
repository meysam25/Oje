using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("Banks")]
    public class Bank
    {
        public Bank()
        {
            ProposalFilledFormDocuments = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [MaxLength(100)]
        public string Pic { get; set; }

        [InverseProperty("Bank")]
        public List<ProposalFilledFormDocument> ProposalFilledFormDocuments { get; set; }
    }
}
