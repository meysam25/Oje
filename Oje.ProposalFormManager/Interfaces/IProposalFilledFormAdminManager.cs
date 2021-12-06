using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormAdminManager
    {
        ApiResult Delete(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus type);
        GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus type);
    }
}
