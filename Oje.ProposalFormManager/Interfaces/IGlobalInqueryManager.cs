using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IGlobalInqueryManager
    {
        void Create(List<GlobalInquery> inputs);
        object GetSumPrice(long inquiryId, int proposalFormId, int? siteSettingId);
        int GetCompanyId(long id, int? siteSettingId);
        long GetSumPriceLong(long id, int proposalFormId, int? siteSettingId);
        bool IsValid(long id, int? siteSettingId, int proposalFormId);
        bool HasAnyCashDiscount(long inQuiryId);
    }
}
