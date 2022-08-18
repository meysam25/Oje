using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabCompanies")]
    public class SanabCompany
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("SanabCompanies")]
        public Company Company { get; set; }
        [Required, MaxLength(30)]
        public string Code { get; set; }
    }
}
