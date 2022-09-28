using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.DB;
using Oje.Section.CarThirdBaseData.Models.View;
using Oje.Section.CarThirdBaseData.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.CarThirdBaseData.Services
{
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService : IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
    {
        readonly CarThirdBaseDataDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService
            (
                CarThirdBaseDataDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount()
            {
                Title = input.title,
                Percent = input.percent.Value,
                Price = input.price.Value,
                SiteSettingId = siteSettingId.Value,
                VehicleTypeId = input.vtId.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.comIds != null)
                foreach (var cid in input.comIds)
                    db.Entry(new ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany() { CompanyId = cid, ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Price);
            if (input.percent.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.vtId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VeicleType);
            if (input.comIds == null || input.comIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (
                    db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts
                        .Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.VehicleTypeId == input.vtId && t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies.Any(tt => input.comIds.Contains(tt.CompanyId)))
                )
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts
              .Include(t => t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
              .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
              .Where(t => t.Id == id)
              .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies != null)
                foreach (var com in foundItem.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts
              .Include(t => t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
              .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
              .Where(t => t.Id == id)
              .Select(t => new 
              {
                  id = t.Id,
                  title = t.Title,
                  price = t.Price,
                  percent = t.Percent,
                  vtId = t.VehicleTypeId,
                  comIds = t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies.Select(tt => tt.CompanyId).ToList()
              })
              .FirstOrDefault();
        }

        public GridResultVM<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGridResultVM> GetList(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (searchInput.percent.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.ctId.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.VehicleTypeId == searchInput.ctId);
            if (searchInput.comId.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies.Any(tt => tt.CompanyId == searchInput.comId));

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGridResultVM>() 
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    price = t.Price,
                    percent = t.Percent,
                    comId = t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    ctId = t.VehicleType.Title
                })
                .ToList()
                .Select(t => new ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    comId = String.Join(',',t.comId),
                    ctId = t.ctId,
                    percent = t.percent.ToString(),
                    price = t.price.ToString("###,###"),
                    title = t.title 
                })
                .ToList()
            };
        }

        public ApiResult Update(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts
                .Include(t => t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies != null)
                foreach (var com in foundItem.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.Price = input.price.Value;
            foundItem.Percent = input.percent.Value;
            foundItem.VehicleTypeId = input.vtId.Value;

            if (input.comIds != null)
                foreach (var cid in input.comIds)
                    db.Entry(new ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany() { CompanyId = cid, ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
