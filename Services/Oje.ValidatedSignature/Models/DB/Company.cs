using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
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
        [MaxLength(100)]
        public string Pic32 { get; set; }
        [MaxLength(100)]
        public string Pic64 { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Company")]
        public List<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
    }
}
