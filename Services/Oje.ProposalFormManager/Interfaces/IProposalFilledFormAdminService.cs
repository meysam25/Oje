using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.ProposalFormService.Models.View;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormAdminService
    {
        ApiResult Delete(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status, List<string> roles);
        string GetJsonConfir(int id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, string loadUrl, string saveUrl);
        object GetById(long? id, int? sitesettingId, long? userId, ProposalFilledFormStatus status);
        object Update(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, IFormCollection form);
        object GetRefferUsers(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult CreateUserRefer(CreateUpdateProposalFilledFormUserReffer input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetCompanies(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UpdateCompanies(CreateUpdateProposalFilledFormCompany input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetAgent(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object UpdateAgent(long? id, long? userId, int? siteSettingId, long? longUserId, ProposalFilledFormStatus status);
        ProposalFilledFormPdfVM PdfDetailes(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null);
        object GetUploadImages(GlobalGridParentLong id, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null);
        ApiResult DeleteUploadImage(long? uploadFileId, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UploadImage(long? proposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null);
        object GetStatus(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UpdateStatus(ProposalFilledFormChangeStatusVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetListForUser(MyProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, List<ProposalFilledFormStatus> validStatus);
        object GetDefaultValuesForIssue(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult IssuePPF(ProposalFilledFormIssueVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ProposalFilledFormPdfVM PdfDetailesByForm(IFormCollection request, int? siteSettingId);
    }
}
