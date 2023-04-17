using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using System.Collections.Generic;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderProposalFormJsonConfigFileService
    {
        ApiResult Create(TenderProposalFormJsonConfigFileCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        List<TenderProposalFormJsonConfigFile> GetFilesBy(int? siteSettingId, int fid);
        string GetJsonConfigBy(int? ppfid, int? siteSettingId);
        GridResultVM<TenderProposalFormJsonConfigFileMainGridResultVM> GetList(TenderProposalFormJsonConfigFileMainGrid searchInput, int? siteSettingId);
        ApiResult Update(TenderProposalFormJsonConfigFileCreateUpdateVM input, int? siteSettingId);
    }
}
