using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IGlobalInqueryService
    {
        void Create(List<GlobalInquery> inputs);
        object GetSumPrice(long inquiryId, int proposalFormId, int? siteSettingId);
        int GetCompanyId(long id, int? siteSettingId);
        long GetSumPriceLong(long id, int proposalFormId, int? siteSettingId);
        bool IsValid(long id, int? siteSettingId, int proposalFormId);
        bool HasAnyCashDiscount(long inQuiryId);
        void AppendInquiryData(long id, List<ProposalFilledFormPdfGroupVM> proposalFilledFormPdfGroupVMs);
    }
}
