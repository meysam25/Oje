using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Question.Models.DB
{
    [Table("ProposalFormYourQuestions")]
    public class ProposalFormYourQuestion: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("ProposalFormYourQuestions")]
        public ProposalForm ProposalForm { get; set; }
        [Required, MaxLength(1000)]
        public string Title { get; set; }
        [Required, MaxLength(4000)]
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public bool? IsInquiry { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ProposalFormYourQuestions")]
        public SiteSetting SiteSetting { get; set; }
    }
}
