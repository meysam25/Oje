using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalFormCategories")]
    public class ProposalFormCategory
    {
        public ProposalFormCategory()
        {
            ProposalForms = new List<ProposalForm>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("ProposalFormCategory")]
        public List<ProposalForm> ProposalForms { get; set; }
    }
}
