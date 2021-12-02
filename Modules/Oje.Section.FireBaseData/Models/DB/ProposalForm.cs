using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            FireInsuranceCoverages = new List<FireInsuranceCoverage>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [InverseProperty("ProposalForm")]
        public List<FireInsuranceCoverage> FireInsuranceCoverages { get; set; }
    }
}
