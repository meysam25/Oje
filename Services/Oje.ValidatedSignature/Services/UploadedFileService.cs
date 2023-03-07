using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UploadedFileService : IUploadedFileService
    {
        readonly ValidatedSignatureDBContext db = null;
        public UploadedFileService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.UploadedFiles
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in UploadedFiles" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<UploadedFileMainGridResultVM> GetList(UploadedFileMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UploadedFiles
                .Include(t => t.CreateByUser)
                .Include(t => t.SiteSetting)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (searchInput.ft != null)
                quiryResult = quiryResult.Where(t => t.FileType == searchInput.ft);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.CreateByUser.Firstname + " " + t.CreateByUser.Lastname).Contains(searchInput.user) || t.CreateByUser.Username.Contains(searchInput.user));
            if (searchInput.rAccess != null)
                quiryResult = quiryResult.Where(t => t.IsFileAccessRequired == searchInput.rAccess);
            if (!string.IsNullOrEmpty(searchInput.fct))
                quiryResult = quiryResult.Where(t => t.FileContentType.Contains(searchInput.fct));
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));
            if (searchInput.fs.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.FileSize == searchInput.fs);

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
                else if (searchInput.sortField == "ft" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.FileType);
                    hasSort = true;
                }
                else if (searchInput.sortField == "ft" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.FileType);
                    hasSort = true;
                }
                else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateByUserId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.CreateByUserId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "rAccess" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.IsFileAccessRequired);
                    hasSort = true;
                }
                else if (searchInput.sortField == "rAccess" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.IsFileAccessRequired);
                    hasSort = true;
                }
                else if (searchInput.sortField == "fct" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.FileContentType);
                    hasSort = true;
                }
                else if (searchInput.sortField == "fct" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.FileContentType);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "fs" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.FileSize);
                    hasSort = true;
                }
                else if (searchInput.sortField == "fs" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.FileSize);
                    hasSort = true;
                }
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);


            int row = searchInput.skip;

            return new GridResultVM<UploadedFileMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new UploadedFileMainGridResultVM
                {
                    row = ++row,
                    fct = t.FileContentType,
                    ft = t.FileType.GetEnumDisplayName(),
                    id = t.Id,
                    rAccess = t.IsFileAccessRequired == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    user = t.CreateByUser != null ? t.CreateByUser.Username + (!string.IsNullOrEmpty(t.CreateByUser.Firstname) ? ("(" + t.CreateByUser.Firstname + " " + t.CreateByUser.Lastname + ")") : "") : "",
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    fs = t.FileSize + "",
                    isValid = t.IsSignature() ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
