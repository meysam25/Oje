using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.Question.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            ProposalFormYourQuestions = new();
            UserRegisterFormYourQuestions = new();
            YourQuestions = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<ProposalFormYourQuestion> ProposalFormYourQuestions { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserRegisterFormYourQuestion> UserRegisterFormYourQuestions { get; set; }
        [InverseProperty("SiteSetting")]
        public List<YourQuestion> YourQuestions { get; set; }
    }
}
