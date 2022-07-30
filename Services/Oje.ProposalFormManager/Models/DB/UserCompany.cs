using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("UserCompanies")]
    public class UserCompany
    {
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserCompanies")]
        public User User { get; set; }
        public int CompanyId { get;set; }
        [ForeignKey("CompanyId"), InverseProperty("UserCompanies")]
        public Company Company { get; set; }
    }
}
