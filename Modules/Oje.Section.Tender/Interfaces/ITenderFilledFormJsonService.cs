using Oje.Section.Tender.Models.DB;
using System.Collections.Generic;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormJsonService
    {
        void Create(long tenderFilledFormId, string tempJsonFile, int? tenderProposalFormJsonConfigId);
        List<TenderFilledFormJson> GetBy(long tenderFilledFormId);
    }
}
