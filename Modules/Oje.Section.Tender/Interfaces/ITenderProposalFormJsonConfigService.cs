using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using System.Collections.Generic;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderProposalFormJsonConfigService
    {
        ApiResult Create(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        TenderProposalFormJsonConfigCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<TenderProposalFormJsonConfigMainGridResultVM> GetList(TenderProposalFormJsonConfigMainGrid searchInput, int? siteSettingId);
        string GetJsonConfigBy(int? proposalFormId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId, int? insuranceCatId);
        List<TenderProposalFormJsonConfig> Validate(int? siteSettingId, List<int> tenderProposalFormJsonConfigIds);
        string GetDocuemntHtml(int? id, int? siteSettingId);
    }
}
