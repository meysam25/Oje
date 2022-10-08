using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormRequiredDocumentTypeService : IProposalFormRequiredDocumentTypeService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        public ProposalFormRequiredDocumentTypeService(ProposalFormBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            CreateValidation(input);

            db.Entry(new ProposalFormRequiredDocumentType()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.formId.Value,
                SiteSettingId = input.siteId,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        private void CreateValidation(CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Type, ApiResultErrorCode.ValidationError);
            if (db.ProposalFormRequiredDocumentTypes.Any(t => t.Id != input.id && t.ProposalFormId == input.formId && t.Title == input.title && t.SiteSettingId == input.siteId))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (!db.ProposalForms.Any(t => t.Id == input.formId && (t.SiteSettingId == null || t.SiteSettingId == input.siteId)))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ProposalFormRequiredDocumentTypes.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public CreateUpdateProposalFormRequiredDocumentTypeVM GetById(int? id)
        {
            return db.ProposalFormRequiredDocumentTypes.Where(t => t.Id == id).Select(t => new CreateUpdateProposalFormRequiredDocumentTypeVM
            {
                id = t.Id,
                formId = t.ProposalFormId,
                formId_Title = t.ProposalForm.Title,
                isActive = t.IsActive,
                siteId = t.SiteSettingId,
                title = t.Title
            }).FirstOrDefault();
        }

        public GridResultVM<ProposalFormRequiredDocumentTypeMainGridResultVM> GetList(ProposalFormRequiredDocumentTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProposalFormRequiredDocumentTypeMainGrid();

            var qureResult = db.ProposalFormRequiredDocumentTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.siteId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.siteId);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.siteId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.siteId);
            if (!string.IsNullOrEmpty(searchInput.form))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.form));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            var row = searchInput.skip;

            return new GridResultVM<ProposalFormRequiredDocumentTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    form = t.ProposalForm.Title,
                    siteId = t.SiteSetting.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormRequiredDocumentTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    form = t.form,
                    siteId = t.siteId,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            CreateValidation(input);

            var foundItem = db.ProposalFormRequiredDocumentTypes.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.SiteSettingId = input.siteId;
            foundItem.Title = input.title;

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetSellect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalFormRequiredDocumentTypes.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search) || t.ProposalForm.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title + "(" + t.ProposalForm.Title + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
