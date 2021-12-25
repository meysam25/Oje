using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            ProposalFilledFormCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Pic { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Company")]
        public List<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }

    }
}
