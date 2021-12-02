using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InqueryDescriptionManager : IInqueryDescriptionManager
    {
        readonly InquiryBaseDataDBContext db = null;
        public InqueryDescriptionManager(
                InquiryBaseDataDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateInqueryDescriptionVM input)
        {
            CreateValidation(input);
            
            var newItem = new InqueryDescription
            {
                Description = input.desc.removeDangerusTags(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.ppfId.Value,
                SiteSettingId = input.settId,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null && input.cIds.Count > 0)
            {
                foreach (var cid in input.cIds)
                {
                    db.Entry(new InqueryDescriptionCompany { CompanyId = cid, InqueryDescriptionId = newItem.Id }).State = EntityState.Added;
                }
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInqueryDescriptionVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.desc))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.desc.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (db.InqueryDescriptions.Any(t => t.SiteSettingId == input.settId && t.ProposalFormId == input.ppfId && t.Id != input.id && t.InqueryDescriptionCompanies.Any(tt => input.cIds.Contains(tt.CompanyId))))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.InqueryDescriptions.Where(t => t.Id == id).Include(t => t.InqueryDescriptionCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.InqueryDescriptionCompanies != null && foundItem.InqueryDescriptionCompanies.Count > 0)
                foreach (var item in foundItem.InqueryDescriptionCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInqueryDescriptionVM GetById(int? id)
        {
            return db.InqueryDescriptions.Where(t => t.Id == id).AsNoTracking().Select(t => new CreateUpdateInqueryDescriptionVM
            {
                id = t.Id,
                cIds = t.InqueryDescriptionCompanies.Select(tt => tt.CompanyId).ToList(),
                desc = t.Description,
                isActive = t.IsActive,
                ppfId = t.ProposalFormId,
                ppfId_Title = t.ProposalForm.Title,
                settId = t.SiteSettingId,
                title = t.Title
            }).FirstOrDefault();
        }

        public GridResultVM<InqueryDescriptionMainGridResultVM> GetList(InqueryDescriptionMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InqueryDescriptionMainGrid();

            var qureResult = db.InqueryDescriptions.AsQueryable();

            if (searchInput.cat.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InqueryDescriptionCompanies.Any(tt => tt.CompanyId == searchInput.cat));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (searchInput.siteSettingId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.siteSettingId);

            var row = searchInput.skip;

            return new GridResultVM<InqueryDescriptionMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    cat = t.InqueryDescriptionCompanies.Select(tt => tt.Company.Title).ToList(),
                    title = t.Title,
                    isActive = t.IsActive,
                    ppfTitle = t.ProposalForm.Title,
                    siteSettingId = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InqueryDescriptionMainGridResultVM
                {
                    cat = string.Join(",", t.cat),
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    ppfTitle = t.ppfTitle,
                    row = ++row,
                    siteSettingId = t.siteSettingId,
                    title = t.title
                }).ToList()
            };
        }

        public ApiResult Update(CreateUpdateInqueryDescriptionVM input)
        {
            CreateValidation(input);

            var foundItem = db.InqueryDescriptions.Where(t => t.Id == input.id).Include(t => t.InqueryDescriptionCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InqueryDescriptionCompanies != null && foundItem.InqueryDescriptionCompanies.Count > 0)
                foreach (var item in foundItem.InqueryDescriptionCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.Description = input.desc;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.ppfId.Value;
            foundItem.SiteSettingId = input.settId;
            foundItem.Title = input.title;

            if (input.cIds != null && input.cIds.Count > 0)
            {
                foreach (var cid in input.cIds)
                {
                    db.Entry(new InqueryDescriptionCompany { CompanyId = cid, InqueryDescriptionId = foundItem.Id }).State = EntityState.Added;
                }
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
