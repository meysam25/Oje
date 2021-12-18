using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Oje.Section.FireBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.FireBaseData.Services.EContext;

namespace Oje.Section.FireBaseData.Services
{
    public class FireInsuranceRateService : IFireInsuranceRateService
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceRateService(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceRateVM input)
        {
            createValidation(input);

            var newItem = new FireInsuranceRate
            {
                FireInsuranceBuildingBodyId = input.bBodyId.Value,
                FireInsuranceBuildingTypeId = input.bTypeId.Value,
                FromPrice = input.minValue.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Rate = input.rate.Value,
                Title = input.title,
                ToPrice = input.maxValue.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new FireInsuranceRateCompany() { CompanyId = cid, FireInsuranceRateId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceRateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.bTypeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BuildingType);
            if (input.bBodyId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BuildingBody);
            if (input.minValue == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_MinValue);
            if (input.minValue < 0)
                throw BException.GenerateNewException(BMessages.MinValue_Can_Not_Be_LessThen_Ziro);
            if (input.maxValue == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxValue);
            if (input.minValue >= input.maxValue)
                throw BException.GenerateNewException(BMessages.MinValue_Can_Not_Be_More_Then_MaxValue);
            if (input.rate == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate);
            if (input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceRates.Where(t => t.Id == id).Include(t => t.FireInsuranceRateCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.FireInsuranceRateCompanies != null)
                foreach (var item in foundItem.FireInsuranceRateCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceRateVM GetById(int? id)
        {
            return db.FireInsuranceRates.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceRateVM
            {
                id = t.Id,
                bBodyId = t.FireInsuranceBuildingBodyId,
                bTypeId = t.FireInsuranceBuildingTypeId,
                isActive = t.IsActive,
                cIds = t.FireInsuranceRateCompanies.Select(tt => tt.CompanyId).ToList(),
                maxValue = t.ToPrice,
                minValue = t.FromPrice,
                rate = t.Rate,
                title = t.Title
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceRateMainGridResultVM> GetList(FireInsuranceRateMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceRateMainGrid();

            var qureResult = db.FireInsuranceRates.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FireInsuranceRateCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (searchInput.bBody.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FireInsuranceBuildingBodyId == searchInput.bBody);
            if (searchInput.bType.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FireInsuranceBuildingTypeId == searchInput.bType);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.maxValue.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ToPrice == searchInput.maxValue);
            if (searchInput.minValue.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FromPrice == searchInput.minValue);
            if (searchInput.rate != null && searchInput.rate > 0)
                qureResult = qureResult.Where(t => t.Rate == searchInput.rate);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<FireInsuranceRateMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    company = t.FireInsuranceRateCompanies.Select(tt => tt.Company.Title).ToList(),
                    bBody = t.FireInsuranceBuildingBody.Title,
                    bType = t.FireInsuranceBuildingType.Title,
                    title = t.Title,
                    maxValue = t.ToPrice,
                    minValue = t.FromPrice,
                    rate = t.Rate,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceRateMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    bBody = t.bBody,
                    bType = t.bType,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name: BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    maxValue = t.maxValue.ToString("###,###"),
                    minValue = t.minValue == 0 ? "0" : t.minValue.ToString("###,###"),
                    rate = t.rate
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceRateVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceRates.Where(t => t.Id == input.id).Include(t => t.FireInsuranceRateCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.FireInsuranceRateCompanies != null)
                foreach (var item in foundItem.FireInsuranceRateCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.FireInsuranceBuildingBodyId = input.bBodyId.Value;
            foundItem.FireInsuranceBuildingTypeId = input.bTypeId.Value;
            foundItem.FromPrice = input.minValue.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Rate = input.rate.Value;
            foundItem.Title = input.title;
            foundItem.ToPrice = input.maxValue.Value;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new FireInsuranceRateCompany() { CompanyId = cid, FireInsuranceRateId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
