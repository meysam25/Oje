using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormPrintDescrptionService
    {
        List<ProposalFormPrintDescrptionVM> GetList(int? siteSettingId, int proposalFormId);
    }
}
