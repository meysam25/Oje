using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            ProposalFilledForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("ProposalForm")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
    }
}
