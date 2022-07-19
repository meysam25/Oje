namespace Oje.Section.Question.Models.View
{
    public class ProposalFormYourQuestionCreateUpdateVM
    {
        public int? id { get; set; }
        public int? fid { get; set; }
        public string title { get; set; }
        public string answer { get; set; }
        public bool? isActive { get; set; }
        public bool? isInquiry { get; set; }
    }
}
