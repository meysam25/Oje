﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderProposalFormJsonConfigService : ITenderProposalFormJsonConfigService
    {
        readonly TenderDBContext db = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService = null;

        public TenderProposalFormJsonConfigService
            (
                TenderDBContext db,
                IProposalFormService ProposalFormService,
                IHttpContextAccessor HttpContextAccessor,
                ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.TenderProposalFormJsonConfigFileService = TenderProposalFormJsonConfigFileService;
        }

        public ApiResult Create(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new TenderProposalFormJsonConfig()
            {
                Description = input.pdfDesc,
                JsonConfig = input.jsonStr,
                ConsultationJsonConfig = input.cJsonStr,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.ppfId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist(input.ppfId, (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.jsonStr))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            if (string.IsNullOrEmpty(input.pdfDesc))
                throw BException.GenerateNewException(BMessages.Please_Enter_PdfDesciption);
            if (db.TenderProposalFormJsonConfigs.Any(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId) && t.Id != input.id && t.ProposalFormId == input.ppfId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TenderProposalFormJsonConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public TenderProposalFormJsonConfigCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.TenderProposalFormJsonConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    ppfId = t.ProposalFormId,
                    ppfId_Title = t.ProposalForm.Title,
                    jsonStr = t.JsonConfig,
                    pdfDesc = t.Description,
                    isActive = t.IsActive,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title,
                    cJsonStr = t.ConsultationJsonConfig
                })
                .ToList()
                .Select(t => new TenderProposalFormJsonConfigCreateUpdateVM
                {
                    id = t.id,
                    isActive = t.isActive,
                    jsonStr = t.jsonStr,
                    pdfDesc = t.pdfDesc,
                    ppfId = t.ppfId,
                    ppfId_Title = t.ppfId_Title,
                    cSOWSiteSettingId = t.cSOWSiteSettingId,
                    cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title,
                    cJsonStr = t.cJsonStr
                })
                .FirstOrDefault();
        }

        public GridResultVM<TenderProposalFormJsonConfigMainGridResultVM> GetList(TenderProposalFormJsonConfigMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TenderProposalFormJsonConfigMainGrid();

            var quiryResult = db.TenderProposalFormJsonConfigs.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                quiryResult = quiryResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

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
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new TenderProposalFormJsonConfigMainGridResultVM
                {
                    row = ++row,
                    ppfTitle = t.ppfTitle,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(TenderProposalFormJsonConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.TenderProposalFormJsonConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.pdfDesc;
            foundItem.JsonConfig = input.jsonStr;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.ppfId.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;
            foundItem.ConsultationJsonConfig = input.cJsonStr;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public string GetJsonConfigBy(int? proposalFormId, int? siteSettingId)
        {
            string tempResult = db.TenderProposalFormJsonConfigs.Where(t => t.IsActive == true && t.Id == proposalFormId && t.SiteSettingId == siteSettingId).Select(t => t.JsonConfig).FirstOrDefault();
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

        public List<TenderProposalFormJsonConfig> Validate(int? siteSettingId, List<UserInputPPF> tenderProposalFormJsonConfigIds)
        {
            var allIds = tenderProposalFormJsonConfigIds.Select(t => t.fid).ToList();
            var result = db.TenderProposalFormJsonConfigs.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && allIds.Contains(t.Id)).ToList();
            if (tenderProposalFormJsonConfigIds == null ||
                tenderProposalFormJsonConfigIds.Count != result.Count)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            return result;
        }

        public string GetDocuemntHtml(int? id, int? siteSettingId)
        {
            return db.TenderProposalFormJsonConfigs.OrderByDescending(t => t.Id).Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.Description).FirstOrDefault();
        }

        public string GetConsultJsonConfig(string id, int? siteSettingId)
        {
            if (!string.IsNullOrEmpty(id) && id.IndexOf("_") > -1)
            {
                int cId = id.Split('_')[1].ToIntReturnZiro();
                if(cId > 0)
                {
                    string resultConfig = db.TenderProposalFormJsonConfigs.Where(t => t.Id == cId && t.SiteSettingId == siteSettingId).Select(t => t.ConsultationJsonConfig).FirstOrDefault();
                    if (!string.IsNullOrEmpty(resultConfig))
                        return resultConfig;
                }
            }
            return "{}";
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new () { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                    db.TenderProposalFormJsonConfigs
                    .Where(t => t.SiteSettingId == siteSettingId)
                    .Select(t => new { id = t.Id, title = t.ProposalForm.Title })
                    .ToList()
                );

            return result;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.TenderProposalFormJsonConfigs.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }
    }
}
