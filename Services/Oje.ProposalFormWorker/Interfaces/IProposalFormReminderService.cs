using System.Collections.Generic;

namespace Oje.ProposalFormWorker.Interfaces
{
    public interface IProposalFormReminderService
    {
        void Notify(List<int> days);
    }
}
