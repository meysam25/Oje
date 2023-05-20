using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.DB;
using Oje.Section.Secretariat.Models.View;
using Oje.Section.Secretariat.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Secretariat.Services
{
    public class SecretariatHeaderFooterService : ISecretariatHeaderFooterService
    {
        readonly static string validExtension = ".jpg,.png,.jpeg";
        readonly SecretariatDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public SecretariatHeaderFooterService
            (
                SecretariatDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(SecretariatHeaderFooterCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new SecretariatHeaderFooter()
            {
                FooterUrl = " ",
                HeaderUrl = " ",
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            if (input.header != null && input.header.Length > 0)
                newItem.HeaderUrl = UploadedFileService.UploadNewFile(FileType.SecretariatHeaderFooter, input.header, null, siteSettingId, newItem.Id, validExtension, false, null, input.title);
            if (input.footer != null && input.footer.Length > 0)
                newItem.FooterUrl = UploadedFileService.UploadNewFile(FileType.SecretariatHeaderFooter, input.footer, null, siteSettingId, newItem.Id, validExtension, false, null, input.title);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatHeaderFooterCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.id.ToIntReturnZiro() <= 0 && (input.header == null || input.header.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.id.ToIntReturnZiro() <= 0 && (input.footer == null || input.footer.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.header != null && input.header.Length > 0 && !input.header.IsValidExtension(validExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
            if (input.footer != null && input.footer.Length > 0 && !input.footer.IsValidExtension(validExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatHeaderFooters.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            try
            {
                db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception) { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SecretariatHeaderFooters
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    t.HeaderUrl,
                    t.FooterUrl
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.title,
                    header_address = GlobalConfig.FileAccessHandlerUrl + t.HeaderUrl,
                    footer_address = GlobalConfig.FileAccessHandlerUrl + t.FooterUrl
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatHeaderFooterMainGridResultVM> GetList(SecretariatHeaderFooterMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatHeaderFooters.Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<SecretariatHeaderFooterMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                })
                .ToList()
                .Select(t => new SecretariatHeaderFooterMainGridResultVM()
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatHeaderFooterCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.SecretariatHeaderFooters.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            if (input.header != null && input.header.Length > 0)
                foundItem.HeaderUrl = UploadedFileService.UploadNewFile(FileType.SecretariatHeaderFooter, input.header, null, siteSettingId, foundItem.Id, validExtension, false, null, input.title);
            if (input.footer != null && input.footer.Length > 0)
                foundItem.FooterUrl = UploadedFileService.UploadNewFile(FileType.SecretariatHeaderFooter, input.footer, null, siteSettingId, foundItem.Id, validExtension, false, null, input.title);

            db.SaveChanges();
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.SecretariatHeaderFooters.Where(t => t.SiteSettingId == siteSettingId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.SecretariatHeaderFooters.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }
    }
}
