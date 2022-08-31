using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormStatusLogFileService : IProposalFilledFormStatusLogFileService
    {
        readonly ProposalFormDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public ProposalFilledFormStatusLogFileService
            (
                ProposalFormDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public void Create(long proposalFilledFormStatusLogId, ProposalFilledFormChangeStatusFileVM file, int? siteSettingId, long? userId)
        {
            if (proposalFilledFormStatusLogId > 0 && file != null && file.mainFile != null && file.mainFile.Length > 0 && !string.IsNullOrEmpty(file.fileType) && siteSettingId.ToIntReturnZiro() > 0)
            {
                db.Entry(new ProposalFilledFormStatusLogFile()
                {
                    ProposalFilledFormStatusLogId = proposalFilledFormStatusLogId,
                    Title = file.fileType,
                    SiteSettingId = siteSettingId.Value,
                    FileUrl = UploadedFileService.UploadNewFile(FileType.ProposalFilledFormLogFile, file.mainFile, userId, siteSettingId, proposalFilledFormStatusLogId, ".jpg,.jpeg,.png,.mp4,.pdf", true, null, file.fileType)
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
