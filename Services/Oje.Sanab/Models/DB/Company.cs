using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            SanabCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<SanabCompany> SanabCompanies { get; set; }
    }
}
