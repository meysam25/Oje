using Oje.Infrastructure.Enums;
using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFormRequiredDocumentManager
    {
        object GetLightList(int? siteSettingID, ProposalFormType type);
        List<ProposalFormRequiredDocument> GetProposalFormRequiredDocuments(int? proposalFormId, int? siteSettingId);
    }
}
