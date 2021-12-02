using Microsoft.AspNetCore.Http;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormDocumentManager
    {
        void CreateChequeArr(long proposalFilledFormId, long proposalFilledFormPrice, int? siteSettingId, List<PaymentMethodDetailesCheckVM> checkArr, IFormCollection form);
    }
}
