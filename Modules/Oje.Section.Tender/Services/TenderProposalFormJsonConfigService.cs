using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderProposalFormJsonConfigService : ITenderProposalFormJsonConfigService
    {
        readonly TenderDBContext db = null;
        readonly IProposalFormService ProposalFormService = null;

        public TenderProposalFormJsonConfigService
            (
                TenderDBContext db,
                IProposalFormService ProposalFormService
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
        }

        public ApiResult Create(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new TenderProposalFormJsonConfig()
            {
                Description = input.pdfDesc,
                JsonConfig = input.jsonStr,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.ppfId.Value,
                SiteSettingId = siteSettingId.Value
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist(input.ppfId, siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.jsonStr))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            if (string.IsNullOrEmpty(input.pdfDesc))
                throw BException.GenerateNewException(BMessages.Please_Enter_PdfDesciption);
            if (db.TenderProposalFormJsonConfigs.Any(t => t.SiteSettingId == siteSettingId && t.Id != input.id && t.ProposalFormId == input.ppfId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TenderProposalFormJsonConfigs.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public TenderProposalFormJsonConfigCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.TenderProposalFormJsonConfigs
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    ppfId = t.ProposalFormId,
                    ppfId_Title = t.ProposalForm.Title,
                    jsonStr = t.JsonConfig,
                    pdfDesc = t.Description,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new TenderProposalFormJsonConfigCreateUpdateVM
                {
                    id = t.id,
                    isActive = t.isActive,
                    jsonStr = t.jsonStr,
                    pdfDesc = t.pdfDesc,
                    ppfId = t.ppfId,
                    ppfId_Title = t.ppfId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<TenderProposalFormJsonConfigMainGridResultVM> GetList(TenderProposalFormJsonConfigMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TenderProposalFormJsonConfigMainGrid();

            var quiryResult = db.TenderProposalFormJsonConfigs.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                quiryResult = quiryResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<TenderProposalFormJsonConfigMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    ppfTitle = t.ProposalForm.Title,
                    isActive = t.IsActive,
                })
                .ToList()
                .Select(t => new TenderProposalFormJsonConfigMainGridResultVM
                {
                    row = ++row,
                    ppfTitle = t.ppfTitle,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.TenderProposalFormJsonConfigs.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.pdfDesc;
            foundItem.JsonConfig = input.jsonStr;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.ppfId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public string GetJsonConfigBy(int? proposalFormId)
        {
            string tempResult = db.TenderProposalFormJsonConfigs.Where(t => t.IsActive == true && t.Id == proposalFormId).Select(t => t.JsonConfig).FirstOrDefault();
            if (string.IsNullOrEmpty(tempResult))
                tempResult = "{}";
            return tempResult;
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId, int? insuranceCatId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.TenderProposalFormJsonConfigs
                .OrderByDescending(t => t.Id)
                .Where(t => t.IsActive == true)
                .Where(t => t.SiteSettingId == siteSettingId);
            if (insuranceCatId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProposalForm.ProposalFormCategoryId == insuranceCatId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.ProposalForm.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public List<TenderProposalFormJsonConfig> Validate(int? siteSettingId, List<int> tenderProposalFormJsonConfigIds)
        {
            var result = db.TenderProposalFormJsonConfigs.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && tenderProposalFormJsonConfigIds.Contains(t.Id)).ToList();
            if (tenderProposalFormJsonConfigIds == null ||
                tenderProposalFormJsonConfigIds.Count != result.Count)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            return result;
        }

        public string GetDocuemntHtml(int? id, int? siteSettingId)
        {
            return db.TenderProposalFormJsonConfigs.OrderByDescending(t => t.Id).Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.Description).FirstOrDefault();
        }
    }
}
