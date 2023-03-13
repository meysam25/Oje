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
    public class BankAccountService : IBankAccountService
    {
        readonly ValidatedSignatureDBContext db = null;
        public BankAccountService(ValidatedSignatureDBContext db)
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.BankAccounts
                .Include(t => t.BankAccountSizpaies)
                .Include(t => t.BankAccountSadads)
                .Include(t => t.BankAccountSeps)
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in " + nameof(BankAccount) + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            if (foundItem.BankAccountSizpaies != null)
            {
                foreach (var item in foundItem.BankAccountSizpaies)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in " + nameof(BankAccountSizpay) + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.BankAccountSadads != null)
            {
                foreach (var item in foundItem.BankAccountSadads)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in " + nameof(BankAccountSadad) + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.BankAccountSeps != null)
            {
                foreach (var user in foundItem.BankAccountSeps)
                {
                    if (!user.IsSignature())
                    {
                        result += "change has been found in " + nameof(BankAccountSep) + Environment.NewLine;
                        result += user.GetSignatureChanges();
                    }
                }
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<BankAccountMainGridResultVM> GetList(BankAccountMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.BankAccounts
                .Include(t => t.BankAccountSizpaies)
                .Include(t => t.BankAccountSadads)
                .Include(t => t.BankAccountSeps)
                .Include(t => t.SiteSetting)
                .Include(t => t.User)
                .AsQueryable();

            if (searchInput.id.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.cardno.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.CardNo == searchInput.cardno);
            if (!string.IsNullOrEmpty(searchInput.shabaNo))
                quiryResult = quiryResult.Where(t => t.ShabaNo.Contains(searchInput.shabaNo));
            if (searchInput.hesabNo.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.HesabNo == searchInput.hesabNo);
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => t.User.Username.Contains(searchInput.user) || (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));

            bool hasSort = false;

            if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Id);
            }
            else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Id);
            }
            else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Title);
            }
            else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Title);
            }
            else if (searchInput.sortField == "cardno" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.CardNo);
            }
            else if (searchInput.sortField == "cardno" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.CardNo);
            }
            else if (searchInput.sortField == "shabaNo" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.ShabaNo);
            }
            else if (searchInput.sortField == "shabaNo" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.ShabaNo);
            }
            else if (searchInput.sortField == "hesabNo" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.HesabNo);
            }
            else if (searchInput.sortField == "hesabNo" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.HesabNo);
            }
            else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.UserId);
            }
            else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.UserId);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.IsActive);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.IsActive);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;

            return new GridResultVM<BankAccountMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new BankAccountMainGridResultVM 
                {
                    row = ++row,
                    id = t.Id,
                    cardno = t.CardNo,
                    hesabNo = t.HesabNo,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    shabaNo = t.ShabaNo,
                    title = t.Title,
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    user = t.User != null 
                    ? 
                        (
                            t.User.Username + (!string.IsNullOrEmpty(t.User.Firstname) ? ("(" + t.User.Firstname + " " + t.User.Lastname + ")") : "")
                        )
                    : 
                    "",
                    isValid = 
                        t.IsSignature() 
                        && (t.BankAccountSizpaies == null || t.BankAccountSizpaies.Count == t.BankAccountSizpaies.Count(tt => tt.IsSignature()))
                        && (t.BankAccountSadads == null || t.BankAccountSadads.Count == t.BankAccountSadads.Count(tt => tt.IsSignature()))
                        && (t.BankAccountSeps == null || t.BankAccountSeps.Count == t.BankAccountSeps.Count(tt => tt.IsSignature()))
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
