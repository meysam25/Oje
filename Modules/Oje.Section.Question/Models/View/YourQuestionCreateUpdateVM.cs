using Oje.Infrastructure.Models;

namespace Oje.Section.Question.Models.View
{
    public class YourQuestionCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string answer { get; set; }
        public bool? isActive { get; set; }
    }
}
