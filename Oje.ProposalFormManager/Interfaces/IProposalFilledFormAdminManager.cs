using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormAdminManager
    {
        ApiResult Delete(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        string GetJsonConfir(int id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, string loadUrl, string saveUrl);
        object GetById(long? id, int? sitesettingId, long? userId, ProposalFilledFormStatus status);
        object Update(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, IFormCollection form);
        object GetRefferUsers(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult CreateUserRefer(CreateUpdateProposalFilledFormUserReffer input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetCompanies(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UpdateCompanies(CreateUpdateProposalFilledFormCompany input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetAgent(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object UpdateAgent(long? id, long? userId, int? siteSettingId, long? longUserId, ProposalFilledFormStatus status);
        ProposalFilledFormPdfVM PdfDetailes(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetUploadImages(GlobalGridParentLong id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult DeleteUploadImage(long? uploadFileId, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UploadImage(long? proposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetStatus(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult UpdateStatus(ProposalFilledFormChangeStatusVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        object GetDefaultValuesForIssue(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
        ApiResult IssuePPF(ProposalFilledFormIssueVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status);
    }
}
