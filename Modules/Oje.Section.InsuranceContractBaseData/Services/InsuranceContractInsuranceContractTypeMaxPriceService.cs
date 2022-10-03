using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractInsuranceContractTypeMaxPriceService : IInsuranceContractInsuranceContractTypeMaxPriceService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractInsuranceContractTypeMaxPriceService(
            InsuranceContractBaseDataDBContext db,
            IInsuranceContractTypeService InsuranceContractTypeService,
            IInsuranceContractService InsuranceContractService,
            IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.InsuranceContractTypeService = InsuranceContractTypeService;
            this.InsuranceContractService = InsuranceContractService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            if (db.InsuranceContractInsuranceContractTypeMaxPrices.Any(t => t.InsuranceContractTypeId == input.typeId && t.InsuranceContractId == input.cid && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

            db.Entry(new InsuranceContractInsuranceContractTypeMaxPrice()
            {
                InsuranceContractId = input.cid.Value,
                InsuranceContractTypeId = input.typeId.Value,
                MaxPrice = input.price.Value,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.typeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (!InsuranceContractService.Exist(input.cid, siteSettingId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!InsuranceContractTypeService.Exist(input.typeId, siteSettingId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);

        }

        public ApiResult Delete(string strId, int? siteSettingId)
        {
            if (string.IsNullOrEmpty(strId))
                throw BException.GenerateNewException(BMessages.Not_Found);
            var arrIds = strId.Split('_').Select(t => t.ToIntReturnZiro()).ToList();
            if (arrIds.Count != 2)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundItem = db.InsuranceContractInsuranceContractTypeMaxPrices
                .Where(t => t.InsuranceContractTypeId == arrIds[0] && t.InsuranceContractId == arrIds[1])
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(string strId, int? siteSettingId)
        {
            if (string.IsNullOrEmpty(strId))
                throw BException.GenerateNewException(BMessages.Not_Found);
            var arrIds = strId.Split('_').Select(t => t.ToIntReturnZiro()).ToList();
            if (arrIds.Count != 2)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return db.InsuranceContractInsuranceContractTypeMaxPrices
                .Where(t => t.InsuranceContractTypeId == arrIds[0] && t.InsuranceContractId == arrIds[1])
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    staticCid = t.InsuranceContractId,
                    staticTypeId = t.InsuranceContractTypeId,
                    typeId = t.InsuranceContractTypeId,
                    cid = t.InsuranceContractId,
                    price = t.MaxPrice
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractInsuranceContractTypeMaxPriceMainGridResultVM> GetList(InsuranceContractInsuranceContractTypeMaxPriceMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new InsuranceContractInsuranceContractTypeMaxPriceMainGrid();

            var quiryResult = db.InsuranceContractInsuranceContractTypeMaxPrices
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.typeId))
                quiryResult = quiryResult.Where(t => t.InsuranceContractType.Title.Contains(searchInput.typeId));
            if (!string.IsNullOrEmpty(searchInput.cid))
                quiryResult = quiryResult.Where(t => t.InsuranceContract.Title.Contains(searchInput.cid));
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.MaxPrice == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractInsuranceContractTypeMaxPriceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.InsuranceContractId)
                .ThenByDescending(t => t.InsuranceContractTypeId)
                .Select(t => new
                {
                    t.InsuranceContractId,
                    InsuranceContractTile = t.InsuranceContract.Title,
                    InsuranceContractTypeTitle = t.InsuranceContractType.Title,
                    t.InsuranceContractTypeId,
                    t.MaxPrice,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractInsuranceContractTypeMaxPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.InsuranceContractTypeId + "_" + t.InsuranceContractId,
                    typeId = t.InsuranceContractTile,
                    cid = t.InsuranceContractTypeTitle,
                    price = t.MaxPrice.ToString("###,###"),
                    siteTitleMN2 = t.siteTitleMN2
                }).ToList()
            };
        }

        public ApiResult Update(InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);



            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    var foundItem = db.InsuranceContractInsuranceContractTypeMaxPrices
                        .Where(t => t.InsuranceContractTypeId == input.staticTypeId && t.InsuranceContractId == input.staticCid && t.SiteSettingId == siteSettingId)
                        .FirstOrDefault();

                    if (foundItem == null)
                        throw BException.GenerateNewException(BMessages.Not_Found);

                    db.Entry(foundItem).State = EntityState.Deleted;
                    db.SaveChanges();
                    Create(input, siteSettingId);
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public long? GetMaxPrice(int? siteSettingId, int insuranceContractTypeId, int? insuranceContractId)
        {
            return db.InsuranceContractInsuranceContractTypeMaxPrices
                .Where(t => t.SiteSettingId == siteSettingId && t.InsuranceContractTypeId == insuranceContractTypeId && t.InsuranceContractId==insuranceContractId)
                .Select(t => t.MaxPrice)
                .FirstOrDefault();
        }
    }
}
