using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UserFilledRegisterFormService : IUserFilledRegisterFormService
    {
        readonly ValidatedSignatureDBContext db = null;
        public UserFilledRegisterFormService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.UserFilledRegisterForms
                .Include(t => t.UserFilledRegisterFormCardPayments)
                .Include(t => t.UserFilledRegisterFormCompanies)
                .Include(t => t.UserFilledRegisterFormJsons)
                .Include(t => t.UserRegisterFormDiscountCodeUses)
                .Include(t => t.UserFilledRegisterFormValues).ThenInclude(t => t.UserFilledRegisterFormKey)
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in UserFilledRegisterForms" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            if (foundItem.UserFilledRegisterFormCardPayments != null)
            {
                foreach (var value in foundItem.UserFilledRegisterFormCardPayments)
                {
                    if (!value.IsSignature())
                    {
                        result += "change has been found in UserFilledRegisterFormCardPayments" + Environment.NewLine;
                        result += value.GetSignatureChanges();
                    }
                }
            }


            if (foundItem.UserFilledRegisterFormCompanies != null)
            {
                foreach (var user in foundItem.UserFilledRegisterFormCompanies)
                {
                    if (!user.IsSignature())
                    {
                        result += "change has been found in UserFilledRegisterFormCompanies" + Environment.NewLine;
                        result += user.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.UserFilledRegisterFormJsons != null)
            {
                foreach (var item in foundItem.UserFilledRegisterFormJsons)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in UserFilledRegisterFormJsons" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.UserRegisterFormDiscountCodeUses != null)
            {
                foreach (var item in foundItem.UserRegisterFormDiscountCodeUses)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in UserRegisterFormDiscountCodeUses" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.UserFilledRegisterFormValues != null)
            {
                foreach (var item in foundItem.UserFilledRegisterFormValues)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in UserFilledRegisterFormValues" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                    if (item.UserFilledRegisterFormKey != null)
                    {
                        if (!item.UserFilledRegisterFormKey.IsSignature())
                        {
                            result += "change has been found in UserFilledRegisterFormKey" + Environment.NewLine;
                            result += item.UserFilledRegisterFormKey.GetSignatureChanges();
                        }
                    }
                }
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<UserFilledRegisterFormMainGridResultVM> GetList(UserFilledRegisterFormMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UserFilledRegisterForms
                .Include(t => t.UserFilledRegisterFormCardPayments)
                .Include(t => t.UserFilledRegisterFormCompanies)
                .Include(t => t.UserFilledRegisterFormJsons)
                .Include(t => t.UserRegisterFormDiscountCodeUses)
                .Include(t => t.UserFilledRegisterFormValues).ThenInclude(t => t.UserFilledRegisterFormKey)
                .Include(t => t.SiteSetting)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.username))
                quiryResult = quiryResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => tt.UserFilledRegisterFormKey.Key == "mobile" && tt.Value.Contains(searchInput.username)));
            if (!string.IsNullOrEmpty(searchInput.firstname))
                quiryResult = quiryResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => (tt.UserFilledRegisterFormKey.Key == "firstName" && tt.Value.Contains(searchInput.firstname)) || (tt.UserFilledRegisterFormKey.Key == "firstNameLegal" && tt.Value.Contains(searchInput.firstname))));
            if (!string.IsNullOrEmpty(searchInput.lastname))
                quiryResult = quiryResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => (tt.UserFilledRegisterFormKey.Key == "lastName" && tt.Value.Contains(searchInput.lastname)) || (tt.UserFilledRegisterFormKey.Key == "lastNameLegal" && tt.Value.Contains(searchInput.lastname))));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));

            bool hasSort = false;

            if (!string.IsNullOrEmpty(searchInput.sortField))
            {
                if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Id);
                    hasSort = true;
                }
                else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Id);
                    hasSort = true;
                }
                else if (searchInput.sortField == "username" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "mobile").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "username" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "mobile").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "firstname" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "firstName" || tt.UserFilledRegisterFormKey.Key == "firstNameLegal").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "firstname" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "firstName" || tt.UserFilledRegisterFormKey.Key == "firstNameLegal").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "lastname" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "lastName" || tt.UserFilledRegisterFormKey.Key == "lastNameLegal").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "lastname" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "lastName" || tt.UserFilledRegisterFormKey.Key == "lastNameLegal").Select(tt => tt.Value).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
                    hasSort = true;
                }
            }

            if (hasSort == false)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;

            return new GridResultVM<UserFilledRegisterFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new UserFilledRegisterFormMainGridResultVM 
                {
                    row = ++row,
                    id = t.Id,
                    createDate = t.CreateDate.ToFaDate(),
                    username = t.UserFilledRegisterFormValues != null ? t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "mobile").Select(tt => tt.Value).FirstOrDefault() : "",
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    firstname = t.UserFilledRegisterFormValues != null ? 
                                (
                                    !string.IsNullOrEmpty(t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "firstName").Select(tt => tt.Value).FirstOrDefault()) ? 
                                    t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "firstName").Select(tt => tt.Value).FirstOrDefault() : 
                                    t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "firstNameLegal").Select(tt => tt.Value).FirstOrDefault()
                                ) : 
                                "",
                    lastname = t.UserFilledRegisterFormValues != null ?
                                (
                                    !string.IsNullOrEmpty(t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "lastName").Select(tt => tt.Value).FirstOrDefault()) ?
                                    t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "lastName").Select(tt => tt.Value).FirstOrDefault() :
                                    t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey?.Key == "lastNameLegal").Select(tt => tt.Value).FirstOrDefault()
                                ) :
                                "",
                    isValid = 
                        t.IsSignature() 
                        && (t.UserFilledRegisterFormCardPayments == null || t.UserFilledRegisterFormCardPayments.Count == t.UserFilledRegisterFormCardPayments.Count(tt => tt.IsSignature()))
                        && (t.UserFilledRegisterFormCompanies == null || t.UserFilledRegisterFormCompanies.Count == t.UserFilledRegisterFormCompanies.Count(tt => tt.IsSignature()))
                        && (t.UserFilledRegisterFormJsons == null || t.UserFilledRegisterFormJsons.Count == t.UserFilledRegisterFormJsons.Count(tt => tt.IsSignature()))
                        && (t.UserFilledRegisterFormValues == null || t.UserFilledRegisterFormValues.Count == t.UserFilledRegisterFormValues.Count(tt => tt.IsSignature() && (tt.UserFilledRegisterFormKey == null || tt.UserFilledRegisterFormKey.IsSignature())))
                        && (t.UserRegisterFormDiscountCodeUses == null || t.UserRegisterFormDiscountCodeUses.Count == t.UserRegisterFormDiscountCodeUses.Count(tt => tt.IsSignature()))
                        ?
                        BMessages.Yes.GetEnumDisplayName() : 
                        BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
