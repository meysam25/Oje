using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class ProposalFilledFormService : IProposalFilledFormService
    {
        readonly ValidatedSignatureDBContext db = null;
        public ProposalFilledFormService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.ProposalFilledForms
                .Include(t => t.ProposalFilledFormValues).ThenInclude(t => t.ProposalFilledFormKey)
                .Include(t => t.ProposalFilledFormUsers)
                .Include(t => t.ProposalFilledFormCompanies)
                .Include(t => t.ProposalFilledFormJsons).ThenInclude(t => t.ProposalFilledFormCacheJson)
                .Include(t => t.ProposalFilledFormDocuments)
                .Include(t => t.ProposalFilledFormStatusLogs).ThenInclude(t => t.ProposalFilledFormStatusLogFiles)
                .Include(t => t.ProposalFilledFormSiteSettings)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in ProposalFilledForm" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }
            if (foundItem.ProposalFilledFormValues != null)
            {
                foreach(var value in foundItem.ProposalFilledFormValues)
                {
                    if (!value.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormValues" + Environment.NewLine;
                        result += value.GetSignatureChanges();
                    }
                    if (value.ProposalFilledFormKey != null && !value.ProposalFilledFormKey.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormKey" + Environment.NewLine;
                        result += value.ProposalFilledFormKey.GetSignatureChanges();
                    }
                }
            }
            if (foundItem.ProposalFilledFormUsers != null)
            {
                foreach(var user in foundItem.ProposalFilledFormUsers)
                {
                    if (!user.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormUsers" + Environment.NewLine;
                        result += user.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.ProposalFilledFormCompanies != null)
            {
                foreach (var item in foundItem.ProposalFilledFormCompanies)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormCompanies" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.ProposalFilledFormJsons != null)
            {
                foreach (var item in foundItem.ProposalFilledFormJsons)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormJsons" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                    if (item.ProposalFilledFormCacheJson != null && !item.ProposalFilledFormCacheJson.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormCacheJson" + Environment.NewLine;
                        result += item.ProposalFilledFormCacheJson.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.ProposalFilledFormDocuments != null)
            {
                foreach (var item in foundItem.ProposalFilledFormDocuments)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormDocuments" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            if (foundItem.ProposalFilledFormStatusLogs != null)
            {
                foreach (var item in foundItem.ProposalFilledFormStatusLogs)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormStatusLogs" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                    if (item.ProposalFilledFormStatusLogFiles != null)
                    {
                        foreach (var subItem in item.ProposalFilledFormStatusLogFiles)
                        {
                            if (!subItem.IsSignature())
                            {
                                result += "change has been found in ProposalFilledFormStatusLogFiles" + Environment.NewLine;
                                result += subItem.GetSignatureChanges();
                            }
                        }
                    }
                }
            }

            if (foundItem.ProposalFilledFormSiteSettings != null)
            {
                foreach (var item in foundItem.ProposalFilledFormSiteSettings)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in ProposalFilledFormSiteSettings" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                }
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<ProposalFilledFormMainGridResultVM> GetList(ProposalFilledFormMainGrid searchInput)
        {
            searchInput = searchInput ?? new ProposalFilledFormMainGrid();

            var quiryResult = db.ProposalFilledForms
                .Include(t => t.ProposalFilledFormValues).ThenInclude(t => t.ProposalFilledFormKey)
                .Include(t => t.ProposalFilledFormUsers)
                .Include(t => t.ProposalFilledFormCompanies).ThenInclude(t => t.Company)
                .Include(t => t.ProposalFilledFormJsons).ThenInclude(t => t.ProposalFilledFormCacheJson)
                .Include(t => t.ProposalFilledFormDocuments)
                .Include(t => t.ProposalFilledFormStatusLogs).ThenInclude(t => t.ProposalFilledFormStatusLogFiles)
                .Include(t => t.ProposalFilledFormSiteSettings)
                .Include(t => t.SiteSetting)
                .Include(t => t.ProposalForm)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.companyTitle))
                quiryResult = quiryResult.Where(t => t.ProposalFilledFormCompanies.Any(tt => tt.Company.Title.Contains(searchInput.companyTitle)));
            if (!string.IsNullOrEmpty(searchInput.formTitle))
                quiryResult = quiryResult.Where(t => t.ProposalForm.Title.Contains(searchInput.formTitle));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));

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
                else if (searchInput.sortField == "companyTitle" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.ProposalFilledFormCompanies.Select(tt => tt.CompanyId).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "companyTitle" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.ProposalFilledFormCompanies.Select(tt => tt.CompanyId).FirstOrDefault());
                    hasSort = true;
                }
                else if (searchInput.sortField == "formTitle" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.ProposalFormId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "formTitle" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.ProposalFormId);
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
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;


            return new GridResultVM<ProposalFilledFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new ProposalFilledFormMainGridResultVM()
                {
                    row = ++row,
                    id = t.Id.ToString(),
                    companyTitle = string.Join(",", t.ProposalFilledFormCompanies != null ? t.ProposalFilledFormCompanies.Select(tt => tt.Company != null ? tt.Company.Title : "").ToList() : new List<string>()),
                    formTitle = t.ProposalForm != null ? t.ProposalForm.Title : "",
                    createDate = t.CreateDate.ToFaDate(),
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    isValid =
                    t.IsSignature()
                    &&
                    (t.ProposalFilledFormSiteSettings == null || t.ProposalFilledFormSiteSettings.Count(tt => tt.IsSignature()) == t.ProposalFilledFormSiteSettings.Count)
                    &&
                    (t.ProposalFilledFormStatusLogs == null || t.ProposalFilledFormStatusLogs.Count(tt => tt.IsSignature() && tt.ProposalFilledFormStatusLogFiles.Count(ttt => ttt.IsSignature()) == tt.ProposalFilledFormStatusLogFiles.Count) == t.ProposalFilledFormStatusLogs.Count)
                    &&
                    (t.ProposalFilledFormDocuments == null || t.ProposalFilledFormDocuments.Count(tt => tt.IsSignature()) == t.ProposalFilledFormDocuments.Count)
                    &&
                    (t.ProposalFilledFormJsons == null || t.ProposalFilledFormJsons.Count(tt => tt.IsSignature() && (tt.ProposalFilledFormCacheJson == null || tt.ProposalFilledFormCacheJson.IsSignature())) == t.ProposalFilledFormJsons.Count)
                    &&
                    (t.ProposalFilledFormCompanies == null || t.ProposalFilledFormCompanies.Count(tt => tt.IsSignature()) == t.ProposalFilledFormCompanies.Count)
                    &&
                    (t.ProposalFilledFormUsers == null || t.ProposalFilledFormUsers.Count(tt => tt.IsSignature()) == t.ProposalFilledFormUsers.Count)
                    &&
                    (t.ProposalFilledFormValues == null || t.ProposalFilledFormValues.Count(tt => tt.IsSignature() && (tt.ProposalFilledFormKey == null || tt.ProposalFilledFormKey.IsSignature())) == t.ProposalFilledFormValues.Count)
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
