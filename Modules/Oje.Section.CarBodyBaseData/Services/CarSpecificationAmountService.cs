using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBodyBaseData.Services.EContext;
using Oje.Section.CarBodyBaseData.Models.View;

namespace Oje.Section.CarBodyBaseData.Services
{
    public class CarSpecificationAmountService : ICarSpecificationAmountService
    {
        readonly CarBodyDBContext db = null;
        public CarSpecificationAmountService(CarBodyDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarSpecificationAmountVM input)
        {
            createValidation(input);

            var newItem = new CarSpecificationAmount()
            {
                Amount = input.price,
                CarSpecificationId = input.carSpecId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MaxAmount = input.maxValue.Value,
                MinAmount = input.minVaue.Value,
                Rate = input.rate
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new CarSpecificationAmountCompany() { CarSpecificationAmountId = newItem.Id, CompanyId = cid }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateCarSpecificationAmountVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.carSpecId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (input.minVaue == null || input.minVaue < 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MinValue);
            if (input.maxValue == null || input.maxValue <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxValue);
            if ((input.rate == null && input.price == null) || (input.rate != null && input.price != null))
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate_Or_Price);
            if (input.rate != null && input.rate < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
            if (input.price != null && input.price <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarSpecificationAmounts.Where(t => t.Id == id).Include(t => t.CarSpecificationAmountCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CarSpecificationAmountCompanies != null && foundItem.CarSpecificationAmountCompanies.Count > 0)
                foreach (var item in foundItem.CarSpecificationAmountCompanies)
                    db.Entry(item).State = EntityState.Deleted;


            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarSpecificationAmountVM GetById(int? id)
        {
            return db.CarSpecificationAmounts
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateCarSpecificationAmountVM
                {
                    id = t.Id,
                    carSpecId = t.CarSpecificationId,
                    carSpecId_Title = t.CarSpecification.Title,
                    cIds = t.CarSpecificationAmountCompanies.Select(tt => tt.CompanyId).ToList(),
                    isActive = t.IsActive,
                    maxValue = t.MaxAmount,
                    minVaue = t.MinAmount,
                    price = t.Amount,
                    rate = t.Rate
                }).FirstOrDefault();
        }

        public GridResultVM<CarSpecificationAmountMainGridResultVM> GetList(CarSpecificationAmountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarSpecificationAmountMainGrid();

            var qureResult = db.CarSpecificationAmounts.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CarSpecificationAmountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.carSpecId))
                qureResult = qureResult.Where(t => t.CarSpecification.Title.Contains(searchInput.carSpecId));
            if (searchInput.minValue != null && searchInput.minValue >= 0)
                qureResult = qureResult.Where(t => t.MinAmount == searchInput.minValue);
            if (searchInput.maxValue != null && searchInput.maxValue > 0)
                qureResult = qureResult.Where(t => t.MaxAmount == searchInput.maxValue);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<CarSpecificationAmountMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    carSpecId = t.CarSpecification.Title,
                    company = t.CarSpecificationAmountCompanies.Select(tt => tt.Company.Title).ToList(),
                    id = t.Id,
                    isActive = t.IsActive,
                    maxValue = t.MaxAmount,
                    minValue = t.MinAmount,
                    t.Rate,
                    t.Amount
                })
                .ToList()
                .Select(t => new CarSpecificationAmountMainGridResultVM
                {
                    row = ++row,
                    carSpecId = t.carSpecId,
                    company = string.Join(",", t.company),
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    maxValue = t.maxValue.ToString("###,###"),
                    minValue = t.minValue == 0 ? "0" : t.minValue.ToString("###,###"),
                    amount = t.Rate > 0 ? t.Rate.ToString() : t.Amount != null ? t.Amount.Value.ToString("###,###") : ""
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarSpecificationAmountVM input)
        {
            createValidation(input);

            var foundItem = db.CarSpecificationAmounts.Where(t => t.Id == input.id).Include(t => t.CarSpecificationAmountCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CarSpecificationAmountCompanies != null && foundItem.CarSpecificationAmountCompanies.Count > 0)
                foreach (var item in foundItem.CarSpecificationAmountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.Amount = input.price;
            foundItem.CarSpecificationId = input.carSpecId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MaxAmount = input.maxValue.Value;
            foundItem.MinAmount = input.minVaue.Value;
            foundItem.Rate = input.rate;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new CarSpecificationAmountCompany() { CarSpecificationAmountId = foundItem.Id, CompanyId = cid }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
