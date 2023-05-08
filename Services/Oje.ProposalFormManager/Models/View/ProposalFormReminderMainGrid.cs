using Oje.Infrastructure.Models;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFormReminderMainGrid: GlobalGrid
    {
        public string fn { get; set; }
        public string ppfTitle { get; set; }
        public long? mobile { get; set; }
        public string sd { get; set; }
        public string td { get; set; }
    }
}
