using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("AgentReffers")]
    public class AgentReffer
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        [Required, MaxLength(200)]
        public string FullName { get; set; }
        [Required, MaxLength(14)]
        public string Mobile { get; set; }
        [Required, MaxLength(4000)]
        public string Address { get; set; }
        public int SiteSettingId { get; set; }
    }
}
