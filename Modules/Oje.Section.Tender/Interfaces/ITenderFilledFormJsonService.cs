using Oje.Section.Tender.Models.DB;
using System.Collections.Generic;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormJsonService
    {
        long Create(long tenderFilledFormId, string tempJsonFile, int? tenderProposalFormJsonConfigId, bool? isConsultation = null);
        List<TenderFilledFormJson> GetBy(long tenderFilledFormId);
    }
}
