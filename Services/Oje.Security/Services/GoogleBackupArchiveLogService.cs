using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class GoogleBackupArchiveLogService : IGoogleBackupArchiveLogService
    {
        readonly SecurityDBContext db = null;
        public GoogleBackupArchiveLogService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public GridResultVM<GoogleBackupArchiveLogMainGridResultVM> GetList(GoogleBackupArchiveLogMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GoogleBackupArchiveLogs.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.message))
                quiryResult = quiryResult.Where(t => t.Message.Contains(searchInput.message));

            int row = searchInput.skip;

            return new GridResultVM<GoogleBackupArchiveLogMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    createDate = t.CreateDate,
                    message = t.Message
                })
                .ToList()
                .Select(t => new GoogleBackupArchiveLogMainGridResultVM
                {
                    id = t.id,
                    type = t.type.GetEnumDisplayName(),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    message = t.message
                })
                .ToList()
            };
        }
    }
}
