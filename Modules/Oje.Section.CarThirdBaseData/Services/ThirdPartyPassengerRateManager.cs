using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Oje.Section.CarThirdBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarThirdBaseData.Services.EContext;

namespace Oje.Section.CarThirdBaseData.Services
{
    public class ThirdPartyPassengerRateService: IThirdPartyPassengerRateService
    {
        readonly CarThirdBaseDataDBContext db = null;
        public ThirdPartyPassengerRateService(CarThirdBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateThirdPartyPassengerRateVM input)
        {
            createValidation(input);

            ThirdPartyPassengerRate newItem = new ThirdPartyPassengerRate()
            {
                CarSpecificationId = input.carSpecId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Rate = input.rate.Value,
                Title = input.title,
                Year = input.year.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    newItem.ThirdPartyPassengerRateCompanies.Add(new ThirdPartyPassengerRateCompany() { CompanyId = cid, ThirdPartyPassengerRateId = newItem.Id });

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateThirdPartyPassengerRateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.carSpecId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.rate == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate);
            if (input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
            if (input.year == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Year);
            if (input.year <= 1000 || input.year >= 2000)
                throw BException.GenerateNewException(BMessages.Invalid_Year);
        }

        public ApiResult Delete(int? id)
        {
            ThirdPartyPassengerRate foundItem = db.ThirdPartyPassengerRates.Where(t => t.Id == id).Include(t => t.ThirdPartyPassengerRateCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.ThirdPartyPassengerRateCompanies != null && foundItem.ThirdPartyPassengerRateCompanies.Count > 0)
                foreach (var com in foundItem.ThirdPartyPassengerRateCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateThirdPartyPassengerRateVM GetById(int? id)
        {
            return db.ThirdPartyPassengerRates.Where(t => t.Id == id).Select(t => new CreateUpdateThirdPartyPassengerRateVM
            {
                id = t.Id,
                carSpecId = t.CarSpecificationId,
                carSpecId_Title = t.CarSpecification.Title,
                cIds = t.ThirdPartyPassengerRateCompanies.Select(tt => tt.CompanyId).ToList(),
                isActive = t.IsActive,
                rate = t.Rate,
                title = t.Title,
                year = t.Year
            }).FirstOrDefault();
        }

        public GridResultVM<ThirdPartyPassengerRateMainGridResultVM> GetList(ThirdPartyPassengerRateMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ThirdPartyPassengerRateMainGrid();

            var qureResult = db.ThirdPartyPassengerRates.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ThirdPartyPassengerRateCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.spec))
                qureResult = qureResult.Where(t => t.CarSpecification.Title.Contains(searchInput.spec));
            if (searchInput.year > 0)
                qureResult = qureResult.Where(t => t.Year == searchInput.year);
            if (searchInput.rate != null && searchInput.rate > 0)
                qureResult = qureResult.Where(t => t.Rate == searchInput.rate);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyPassengerRateMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.ThirdPartyPassengerRateCompanies.Select(tt => tt.Company.Title).ToList(),
                    title = t.Title,
                    spec = t.CarSpecification.Title,
                    year = t.Year,
                    rate = t.Rate,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ThirdPartyPassengerRateMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    title = t.title,
                    spec = t.spec,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    year = t.year,
                    rate = t.rate
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateThirdPartyPassengerRateVM input)
        {
            createValidation(input);

            ThirdPartyPassengerRate foundItem = db.ThirdPartyPassengerRates.Where(t => t.Id == input.id).Include(t => t.ThirdPartyPassengerRateCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.ThirdPartyPassengerRateCompanies != null && foundItem.ThirdPartyPassengerRateCompanies.Count > 0)
                foreach (var com in foundItem.ThirdPartyPassengerRateCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            foundItem.CarSpecificationId = input.carSpecId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Rate = input.rate.Value;
            foundItem.Title = input.title;
            foundItem.Year = input.year.Value;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new ThirdPartyPassengerRateCompany() { CompanyId = cid, ThirdPartyPassengerRateId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
