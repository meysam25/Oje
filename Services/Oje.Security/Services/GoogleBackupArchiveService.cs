using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class GoogleBackupArchiveService: IGoogleBackupArchiveService
    {
        readonly SecurityDBContext db = null;
        public GoogleBackupArchiveService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public GridResultVM<GoogleBackupArchiveMainGridResultVM> GetList(GoogleBackupArchiveMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GoogleBackupArchives.AsQueryable();

            if (searchInput.location != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.location);
            if(!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new GridResultVM<GoogleBackupArchiveMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                {
                    t.Id,
                    t.FileSize,
                    t.CreateDate,
                    t.Type
                })
                .ToList()
                .Select(t => new GoogleBackupArchiveMainGridResultVM 
                { 
                    row = ++row,
                    id = t.Id,
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("HH:mm"),
                    size = (t.FileSize / 1024 / 1024) + " mb",
                    location = t.Type != null ? t.Type.Value.GetEnumDisplayName() : "گوگل"
                })
                .ToList()
            };
        }
    }
}
