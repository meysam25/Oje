using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.DB;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        readonly ValidatedSignatureDBContext db = null;
        public SiteSettingService(ValidatedSignatureDBContext db)
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.SiteSettings
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in " + nameof(SiteSetting) + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<SiteSettingMainGridResultVM> GetList(SiteSettingMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SiteSettings
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.domain))
                quiryResult = quiryResult.Where(t => t.WebsiteUrl.Contains(searchInput.domain));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            bool hasSort = false;
            if (!string.IsNullOrEmpty(searchInput.sortField))
            {
                if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Id);
                    hasSort = true;
                }
                else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Id);
                    hasSort = true;
                }
                else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Title);
                    hasSort = true;
                }
                else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Title);
                    hasSort = true;
                }
                else if (searchInput.sortField == "domain" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.WebsiteUrl);
                    hasSort = true;
                }
                else if (searchInput.sortField == "domain" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.WebsiteUrl);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.IsActive);
                    hasSort = true;
                }
                else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.IsActive);
                    hasSort = true;
                }
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;


            return new GridResultVM<SiteSettingMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new SiteSettingMainGridResultVM()
                {
                    row = ++row,
                    createDate = t.CreateDate.ToFaDate(),
                    domain = t.WebsiteUrl,
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    isValid =
                    t.IsSignature()
                        ?
                        BMessages.Yes.GetEnumDisplayName()
                        :
                        BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
