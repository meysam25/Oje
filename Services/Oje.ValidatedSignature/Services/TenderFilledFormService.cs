using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class TenderFilledFormService : ITenderFilledFormService
    {
        ValidatedSignatureDBContext db = null;
        public TenderFilledFormService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.TenderFilledForms
                .Include(t => t.TenderFilledFormIssues)
                .Include(t => t.TenderFilledFormJsons)
                .Include(t => t.TenderFilledFormPFs)
                .Include(t => t.TenderFilledFormPrices)
                .Include(t => t.TenderFilledFormsValues).ThenInclude(t => t.TenderFilledFormKey)
                .Include(t => t.TenderFilledFormValidCompanies)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in TenderFilledForms" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            if (foundItem.TenderFilledFormIssues != null)
            {
                foreach (var item in foundItem.TenderFilledFormIssues)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormIssues" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.TenderFilledFormJsons != null)
            {
                foreach (var item in foundItem.TenderFilledFormJsons)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormJsons" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.TenderFilledFormPFs != null)
            {
                foreach (var item in foundItem.TenderFilledFormPFs)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormPFs" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.TenderFilledFormPrices != null)
            {
                foreach (var item in foundItem.TenderFilledFormPrices)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormPrices" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.TenderFilledFormValidCompanies != null)
            {
                foreach (var item in foundItem.TenderFilledFormValidCompanies)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormValidCompanies" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.TenderFilledFormsValues != null)
            {
                foreach (var item in foundItem.TenderFilledFormsValues)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormsValues" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }

                    if (item.TenderFilledFormKey != null && !item.TenderFilledFormKey.IsSignature())
                    {
                        result += "change has been found in TenderFilledFormKey" + Environment.NewLine;
                        result += item.TenderFilledFormKey.GetSignatureChanges();
                    }
                }
            }



            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<TenderFilledFormMainGridResultVM> GetList(TenderFilledFormMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.TenderFilledForms
                .Include(t => t.TenderFilledFormIssues)
                .Include(t => t.TenderFilledFormJsons)
                .Include(t => t.TenderFilledFormPFs)
                .Include(t => t.TenderFilledFormPrices)
                .Include(t => t.TenderFilledFormsValues).ThenInclude(t => t.TenderFilledFormKey)
                .Include(t => t.TenderFilledFormValidCompanies)
                .Include(t => t.SiteSetting)
                .Include(t => t.User)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user) || t.User.Username.Contains(searchInput.user));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));

            bool hasOrder = false;

            if (!string.IsNullOrEmpty(searchInput.sortField))
            {
                if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderBy(t => t.Id);
                }
                else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderByDescending(t => t.Id);
                }
                else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == true)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderBy(t => t.UserId);
                }
                else if (searchInput.sortField == "user" && searchInput.sortFieldIsAsc == false)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderByDescending(t => t.UserId);
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
                {
                    hasOrder = true;
                    quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
                }
            }

            if (!hasOrder)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;

            return new GridResultVM<TenderFilledFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new TenderFilledFormMainGridResultVM()
                {
                    createDate = t.CreateDate.ToFaDate(),
                    id = t.Id,
                    row = ++row,
                    user = t.User != null ? t.User.Username + ( !string.IsNullOrEmpty(t.User.Lastname) ? ("(" + t.User.Firstname + " " + t.User.Lastname + ")") : "" ) : "",
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    isValid = 
                    t.IsSignature()
                    && (t.TenderFilledFormIssues == null || t.TenderFilledFormIssues.Count == t.TenderFilledFormIssues.Count(tt => tt.IsSignature()))
                    && (t.TenderFilledFormJsons == null || t.TenderFilledFormJsons.Count == t.TenderFilledFormJsons.Count(tt => tt.IsSignature()))
                    && (t.TenderFilledFormPFs == null || t.TenderFilledFormPFs.Count == t.TenderFilledFormPFs.Count(tt => tt.IsSignature()))
                    && (t.TenderFilledFormPrices == null || t.TenderFilledFormPrices.Count == t.TenderFilledFormPrices.Count(tt => tt.IsSignature()))
                    && (t.TenderFilledFormValidCompanies == null || t.TenderFilledFormValidCompanies.Count == t.TenderFilledFormValidCompanies.Count(tt => tt.IsSignature()))
                    && (t.TenderFilledFormsValues == null || t.TenderFilledFormsValues.Count == t.TenderFilledFormsValues.Count(tt => tt.IsSignature() && (tt.TenderFilledFormKey == null || tt.TenderFilledFormKey.IsSignature()) ))
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
