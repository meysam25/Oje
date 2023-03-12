using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class WalletTransactionService : IWalletTransactionService
    {
        readonly ValidatedSignatureDBContext db = null;
        public WalletTransactionService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.WalletTransactions
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in WalletTransaction" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<WalletTransactionMainGridResultVM> GetList(WalletTransactionMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quieryResult = db.WalletTransactions
                .Include(t => t.User)
                .Include(t => t.SiteSetting)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quieryResult = quieryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.user))
                quieryResult = quieryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user) || t.User.Username.Contains(searchInput.user));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quieryResult = quieryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.price.ToLongReturnZiro() > 0)
                quieryResult = quieryResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.traceNo))
                quieryResult = quieryResult.Where(t => t.TraceNo.Contains(searchInput.traceNo));
            if (!string.IsNullOrEmpty(searchInput.website))
                quieryResult = quieryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));

            bool hasSort = false;

            if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.Id);
            }
            else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.Id);
            }
            else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.UserId);
            }
            else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.UserId);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.CreateDate);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.CreateDate);
            }
            else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.Price);
            }
            else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.Price);
            }
            else if (searchInput.sortField == "traceNo" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.TraceNo);
            }
            else if (searchInput.sortField == "traceNo" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.TraceNo);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderBy(t => t.SiteSettingId);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quieryResult = quieryResult.OrderByDescending(t => t.SiteSettingId);
            }

            if (!hasSort)
                quieryResult = quieryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;

            return new GridResultVM<WalletTransactionMainGridResultVM>()
            {
                total = quieryResult.Count(),
                data = quieryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new WalletTransactionMainGridResultVM
                {
                    id = t.Id,
                    createDate = t.CreateDate.ToFaDate(),
                    price = t.Price.ToString("###,###"),
                    row = ++row,
                    traceNo = t.TraceNo,
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    user = t.User != null 
                            ? 
                            (
                                t.User.Username + 
                                                (
                                                    !string.IsNullOrEmpty(t.User.Firstname) 
                                                    ? 
                                                    ("(" + t.User.Firstname + " " + t.User.Lastname + ")") 
                                                    : 
                                                    ""
                                                )
                            ) 
                            : 
                            "",
                    isValid = t.IsSignature() ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
