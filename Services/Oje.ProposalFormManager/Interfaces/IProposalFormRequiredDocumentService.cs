using Oje.Infrastructure.Enums;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormRequiredDocumentService
    {
        object GetLightList(int? siteSettingID, int? proposalFormId);
        List<ProposalFormRequiredDocument> GetProposalFormRequiredDocuments(int? proposalFormId, int? siteSettingId);
    }
}
