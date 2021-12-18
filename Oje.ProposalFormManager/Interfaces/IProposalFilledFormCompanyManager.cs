using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormCompanyService
    {
        void Create(long inquiryId, int? siteSettingId, long proposalFilledFormId, long proposalFilledFormPrice, int companyId, bool isSelected, long? loginUserId);
        void Create(string companyIds, long proposalFilledFormId, long? loginUserId);
        void CreateUpdate(CreateUpdateProposalFilledFormCompany input, long? loginUserId);
        GridResultVM<ProposalFilledFormCompanyPriceMainGridResultVM> GetList(ProposalFilledFormCompanyPriceMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult Delete(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult Create(CreateUpdateProposalFilledFormCompanyPrice input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetBy(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object Update(CreateUpdateProposalFilledFormCompanyPrice input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object Select(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        Company GetSelectedBy(long proposalFilledFormId);
        bool IsSelectedBy(long proposalFilledFormId);
    }
}
