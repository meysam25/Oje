using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderConfigService : ITenderConfigService
    {
        readonly TenderDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        static TenderConfigCreateUpdateVM TitleAndSubTitleCache = null;
        public TenderConfigService
            (
                TenderDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult CreateUpdate(TenderConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.TenderConfigs.Where(t => t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
            {
                foundItem = new Models.DB.TenderConfig() { SiteSettingId = siteSettingId.Value };
                db.Entry(foundItem).State = EntityState.Added;
            }

            foundItem.GeneralRoles = input.desctpion;
            foundItem.PrivateDocumentUrl = " ";
            foundItem.Title = input.title;
            foundItem.SubTitle = input.subTitle;

            db.SaveChanges();

            if (input.generallow != null && input.generallow.Length > 0)
                foundItem.PrivateDocumentUrl = UploadedFileService.UploadNewFile(FileType.TenderGeneralLow, input.generallow, null, siteSettingId, foundItem.Id, ".jpg,.jpeg,.bmp,.pdf,.doc,.docx", false);

            db.SaveChanges();

            TitleAndSubTitleCache = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.desctpion))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.desctpion.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.id.ToIntReturnZiro() <= 0 && (input.generallow == null || input.generallow.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_General_Low_File);
            if (input.generallow != null && input.generallow.Length > 0 && input.generallow.IsValidExtension(".jpg,.jpeg,.bmp,.pdf,.doc,.docx") == false)
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
        }

        public TenderConfigCreateUpdateVM GetBy(int? siteSettingId)
        {
            return db.TenderConfigs
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    t.Id,
                    t.GeneralRoles,
                    t.PrivateDocumentUrl,
                    t.Title,
                    t.SubTitle
                })
                .ToList()
                .Select(t => new TenderConfigCreateUpdateVM
                {
                    id = t.Id,
                    desctpion = t.GeneralRoles,
                    generallow_address = !string.IsNullOrEmpty((t.PrivateDocumentUrl + "").Trim()) ? GlobalConfig.FileAccessHandlerUrl + t.PrivateDocumentUrl : "",
                    title = t.Title,
                    subTitle = t.SubTitle
                })
                .FirstOrDefault();
        }

        public TenderConfigCreateUpdateVM GetTitleAndSubTitleCache(int siteSettingId)
        {
            if (TitleAndSubTitleCache == null)
                TitleAndSubTitleCache = db.TenderConfigs.Where(t => t.SiteSettingId == siteSettingId).Select(t => new TenderConfigCreateUpdateVM { title = t.Title, subTitle = t.SubTitle }).FirstOrDefault();


            return TitleAndSubTitleCache;
        }
    }
}
