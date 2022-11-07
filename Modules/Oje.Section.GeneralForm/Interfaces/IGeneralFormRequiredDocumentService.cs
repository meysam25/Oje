using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormRequiredDocumentService
    {
        ApiResult Create(GeneralFormRequiredDocumentCreateUpdateVM input);
        ApiResult Delete(long? id);
        object GetById(long? id);
        GridResultVM<GeneralFormRequiredDocumentMainGridResult> GetList(GeneralFormRequiredDocumentMainGrid searchInput);
        ApiResult Update(GeneralFormRequiredDocumentCreateUpdateVM input);

        List<GeneralFormRequiredDocument> GetActiveListNoTracking(long generalFormId);
        List<GeneralFormRequiredDocumentVM> GetActiveForWeb(int? generalFormId, int? siteSettingId);
    }
}
