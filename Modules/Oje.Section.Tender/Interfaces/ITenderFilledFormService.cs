using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormService
    {
        ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId);
        TenderFilledFormPdfVM PdfDetailes(long id, int? siteSettingId, long? loginUserId);
        string GetDocument(long? id, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId);
        GridResultVM<MyTenderFilledFormMainGridResultVM> GetListForWeb(MyTenderFilledFormMainGrid searchInput, int? siteSettingId, long? loginUserId);
        GridResultVM<TenderFilledFormMainGridResultVM> GetList(TenderFilledFormMainGrid searchInput, int? siteSettingId, long? userId);
        ApiResult UpdateDatesForWeb(MyTenderFilledFormUpdateDateVM input, int? siteSettingId, long? loginUserId);
        ApiResult UpdateDates(MyTenderFilledFormUpdateDateVM input, int? siteSettingId);
        object GetDatesByForWeb(long? id, int? siteSettingId, long? loginUserId);
        object GetDatesBy(long? id, int? siteSettingId);
        object GetAgentAccessByForWeb(long? id, int? siteSettingId, long? loginUserId);
        object GetAgentAccessBy(long? id, int? siteSettingId);
        object UpdateAgentAccessForWeb(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId, long? loginUserId);
        object UpdateAgentAccess(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId);
        object UpdatePublishForWeb(long? id, int? siteSettingId, long? loginUserId);
        object UpdatePublish(long? id, int? siteSettingId);
        object GetInsuranceList(long? tenderFilledFormId, int? siteSettingId);
        bool ExistByJCId(int? siteSettingId, long? id, int? tenderProposalFormJsonConfigId);
        bool IsPublished(int? siteSettingId, long? id);
        bool ValidateCompany(int? siteSettingId, long? id, int? companyId);
        bool ValidateProvinceAndCity(int? siteSettingId, long? id, int? provinceId, int? cityId);
        bool ValidateOpenCloseDate(int? siteSettingId, long? id);
        long? GetUserId(int? siteSettingId, long? id);
        int GetInsuranceCount(long? id, int? siteSettingId, long? loginUserId);
    }
}
