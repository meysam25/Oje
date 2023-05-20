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
    public class SecretariatLetterCategoryService : ISecretariatLetterCategoryService
    {
        readonly SecretariatDBContext db = null;
        readonly ISecretariatHeaderFooterService SecretariatHeaderFooterService = null;
        readonly ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService = null;
        public SecretariatLetterCategoryService
            (
                SecretariatDBContext db,
                ISecretariatHeaderFooterService SecretariatHeaderFooterService,
                ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService
            )
        {
            this.db = db;
            this.SecretariatHeaderFooterService = SecretariatHeaderFooterService;
            this.SecretariatHeaderFooterDescriptionService = SecretariatHeaderFooterDescriptionService;
        }

        public ApiResult Create(SecretariatLetterCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new SecretariatLetterCategory()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                SecretariatHeaderFooterId = input.hfid.Value,
                SecretariatHeaderFooterDescriptionId = input.hfdid.Value,
                Code = input.code
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatLetterCategoryCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.hfid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_HeaderFooter);
            if (input.hfdid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_HeaderFooterDescription);
            if (!SecretariatHeaderFooterService.Exist(siteSettingId, input.hfid))
                throw BException.GenerateNewException(BMessages.Please_Select_HeaderFooter);
            if (!SecretariatHeaderFooterDescriptionService.Exist(siteSettingId, input.hfdid))
                throw BException.GenerateNewException(BMessages.Please_Select_HeaderFooterDescription);
            if (db.SecretariatLetterCategories.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Code.Trim() == input.code.Trim()))
                throw BException.GenerateNewException(BMessages.Dublicate_ECode);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatLetterCategories.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
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
            return db.SecretariatLetterCategories
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    hfid = t.SecretariatHeaderFooterId,
                    hfdid = t.SecretariatHeaderFooterDescriptionId,
                    code = t.Code
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatLetterCategoryMainGridResultVM> GetList(SecretariatLetterCategoryMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatLetterCategories.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code == searchInput.code);

            int row = searchInput.skip;
            return new GridResultVM<SecretariatLetterCategoryMainGridResultVM>()
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
                    isActive = t.IsActive,
                    t.Code
                })
                .ToList()
                .Select(t => new SecretariatLetterCategoryMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    code = t.Code,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatLetterCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.SecretariatLetterCategories.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.SecretariatHeaderFooterId = input.hfid.Value;
            foundItem.SecretariatHeaderFooterDescriptionId = input.hfdid.Value;
            foundItem.Code = input.code;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.SecretariatLetterCategories.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsActive == true);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                (
                    db.SecretariatLetterCategories
                    .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                    .Select(t => new 
                    {
                        id = t.Id,
                        title = t.Title
                    })
                    .ToList()
                );

            return result;
        }
    }
}
