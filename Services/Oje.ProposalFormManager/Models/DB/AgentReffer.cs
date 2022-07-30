using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("AgentReffers")]
    public class AgentReffer
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("AgentReffers")]
        public Company Company { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        [Required, MaxLength(200)]
        public string FullName { get; set; }
        [Required, MaxLength(14)]
        public string Mobile { get; set; }
        [Required, MaxLength(4000)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        public int SiteSettingId { get; set; }
    }
}
