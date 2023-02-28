using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormService
    {
        ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId);
        TenderFilledFormPdfVM PdfDetailes(long id, int? siteSettingId, long? loginUserId, TenderSelectStatus? selectStatus = null);
        string GetDocument(long? id, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId, TenderSelectStatus? selectStatus = null);
        GridResultVM<MyTenderFilledFormMainGridResultVM> GetListForWeb(MyTenderFilledFormMainGrid searchInput, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        GridResultVM<TenderFilledFormMainGridResultVM> GetList(TenderFilledFormMainGrid searchInput, int? siteSettingId, long? userId, TenderSelectStatus? selectStatus = null);
        ApiResult UpdateDatesForWeb(MyTenderFilledFormUpdateDateVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        ApiResult UpdateDates(MyTenderFilledFormUpdateDateVM input, int? siteSettingId);
        object GetDatesByForWeb(long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object GetDatesBy(long? id, int? siteSettingId);
        object GetAgentAccessByForWeb(long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object GetAgentAccessBy(long? id, int? siteSettingId);
        object UpdateAgentAccessForWeb(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object UpdateAgentAccess(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId);
        object UpdatePublishForWeb(long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object UpdatePublish(long? id, int? siteSettingId);
        object GetInsuranceList(long? tenderFilledFormId, int? siteSettingId, TenderSelectStatus? selectStatus = null);
        bool ExistByJCId(int? siteSettingId, long? id, int? tenderProposalFormJsonConfigId);
        bool IsPublished(int? siteSettingId, long? id);
        bool ValidateCompany(int? siteSettingId, long? id, int? companyId);
        bool ValidateProvinceAndCity(int? siteSettingId, long? id, int? provinceId, int? cityId);
        bool ValidateOpenCloseDate(int? siteSettingId, long? id);
        long? GetUserId(int? siteSettingId, long? id);
        int GetInsuranceCount(long? id, int? siteSettingId, long? loginUserId);
        ApiResult CreateUpdateConsultationValue(int? siteSettingId, IFormCollection form, long? loginUserId, string pKey, TenderSelectStatus selectStatus);
        object GetConsultationValue(int? siteSettingId, IFormCollection form, long? loginUserId, string pKey, TenderSelectStatus selectStatus);
        ApiResult ConfirmPfs(int? siteSettingId, long? loginUserId, string id, TenderSelectStatus selectStatus);
        ApiResult ConfirmPfsForUser(int? siteSettingId, long? loginUserId, string id, TenderSelectStatus selectStatus);
        object GetUploadFiles(GlobalGridParentLong input, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object DeleteUploadFile(long? fileId, long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
        object UploadNewFile(long? id, IFormFile mainFile, string title, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus);
    }
}
