using Oje.Infrastructure.Models;

namespace Oje.Section.Question.Models.View
{
    public class ProposalFormYourQuestionMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string form { get; set; }
        public bool? isActive { get; set; }
    }
}
