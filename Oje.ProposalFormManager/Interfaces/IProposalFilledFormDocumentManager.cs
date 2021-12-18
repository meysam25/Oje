using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormDocumentService
    {
        void CreateChequeArr(long proposalFilledFormId, long proposalFilledFormPrice, int? siteSettingId, List<PaymentMethodDetailesCheckVM> checkArr, IFormCollection form);
        GridResultVM<ProposalFilledFormDocumentMainGridResultVM> GetList(ProposalFilledFormDocumentMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult Delete(long? id, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetBy(long? id, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult Create(ProposalFilledFormDocumentCreateUpdateVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult Update(ProposalFilledFormDocumentCreateUpdateVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
    }
}
