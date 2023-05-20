using Microsoft.EntityFrameworkCore;
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
    public class SecretariatHeaderFooterDescriptionService : ISecretariatHeaderFooterDescriptionService
    {
        readonly SecretariatDBContext db = null;
        public SecretariatHeaderFooterDescriptionService
            (
                SecretariatDBContext db
            )
        {
            this.db = db;
        }
        public ApiResult Create(SecretariatHeaderFooterDescriptionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new SecretariatHeaderFooterDescription()
            {
                Footer = input.footer,
                Header = input.header,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatHeaderFooterDescriptionCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.header) && input.header.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (!string.IsNullOrEmpty(input.footer) && input.footer.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatHeaderFooterDescriptions.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            try
            {
                db.Entry(foundItem).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception) { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SecretariatHeaderFooterDescriptions
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new SecretariatHeaderFooterDescriptionCreateUpdateVM
                {
                    id = t.Id,
                    footer = t.Footer,
                    header = t.Header,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatHeaderFooterDescriptionMainGridResultVM> GetList(SecretariatHeaderFooterDescriptionMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatHeaderFooterDescriptions.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));

            var row = searchInput.skip;

            return new GridResultVM<SecretariatHeaderFooterDescriptionMainGridResultVM>()
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
                .Select(t => new SecretariatHeaderFooterDescriptionMainGridResultVM
                {
                    id = t.id,
                    row = ++row,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatHeaderFooterDescriptionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.SecretariatHeaderFooterDescriptions.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Header = input.header;
            foundItem.Footer = input.footer;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.SecretariatHeaderFooterDescriptions.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.SecretariatHeaderFooterDescriptions.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }
    }
}
