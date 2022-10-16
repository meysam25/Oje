using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class CarExteraDiscountRangeAmountService : ICarExteraDiscountRangeAmountService
    {
        readonly CarDBContext db = null;
        public CarExteraDiscountRangeAmountService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarExteraDiscountRangeAmountVM input)
        {
            CreateValidation(input);

            var newItem = new CarExteraDiscountRangeAmount()
            {
                Amount = input.price,
                CarExteraDiscountId = input.carExteraDiscountId.Value,
                CarExteraDiscountValueId = input.carExteraDiscountValueId,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MaxValue = input.maxValue.Value,
                MinValue = input.minValue.Value,
                Percent = input.percent,
                Title = input.title,
                CreateDateSelfPercent = input.cdSelfPercent
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null && input.cIds.Count > 0)
                foreach (var cId in input.cIds)
                    db.Entry(new CarExteraDiscountRangeAmountCompany() { CompanyId = cId, CarExteraDiscountRangeAmountId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateCarExteraDiscountRangeAmountVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.carExteraDiscountId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarExteraDisocunt);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.minValue == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_MinValue);
            if (input.minValue < 0)
                throw BException.GenerateNewException(BMessages.MinValue_Can_Not_Be_LessThen_Ziro);
            if (input.maxValue == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxValue);
            if (input.percent != null && input.percent == 0)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (input.price != null && input.price == 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (input.price != null && input.percent != null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.price == null && input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.minValue > input.maxValue)
                throw BException.GenerateNewException(BMessages.MinValue_Can_Not_Be_More_Then_MaxValue);
            if (db.CarExteraDiscountRangeAmounts
                .Any(t => 
                        t.Title == input.title && t.CarExteraDiscountId == input.carExteraDiscountId && 
                        t.CarExteraDiscountValueId == input.carExteraDiscountValueId && t.Id != input.id
                    )
                )
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarExteraDiscountRangeAmounts.Where(t => t.Id == id).Include(t => t.CarExteraDiscountRangeAmountCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CarExteraDiscountRangeAmountCompanies != null && foundItem.CarExteraDiscountRangeAmountCompanies.Count > 0)
                foreach (var company in foundItem.CarExteraDiscountRangeAmountCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarExteraDiscountRangeAmountVM GetById(int? id)
        {
            return db.CarExteraDiscountRangeAmounts.Where(t => t.Id == id).Select(t => new CreateUpdateCarExteraDiscountRangeAmountVM
            {
                id = t.Id,
                carExteraDiscountId = t.CarExteraDiscountId,
                carExteraDiscountId_Title = t.CarExteraDiscount.Title,
                carExteraDiscountValueId = t.CarExteraDiscountValueId,
                cIds = t.CarExteraDiscountRangeAmountCompanies.Select(tt => tt.CompanyId).ToList(),
                isActive = t.IsActive,
                maxValue = t.MaxValue,
                minValue = t.MinValue,
                percent = t.Percent,
                price = t.Amount,
                title = t.Title,
                cdSelfPercent = t.CreateDateSelfPercent
            }).FirstOrDefault();
        }

        public GridResultVM<CarExteraDiscountRangeAmountMainGridResultVM> GetList(CarExteraDiscountRangeAmountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarExteraDiscountRangeAmountMainGrid();

            var qureResult = db.CarExteraDiscountRangeAmounts.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CarExteraDiscountRangeAmountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.exteraDiscountTitle))
                qureResult = qureResult.Where(t => t.CarExteraDiscount.Title.Contains(searchInput.exteraDiscountTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.minValue != null && searchInput.minValue.ToLongReturnZiro() >= 0)
                qureResult = qureResult.Where(t => t.MinValue == searchInput.minValue);
            if (searchInput.maxValue.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.MaxValue == searchInput.maxValue);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<CarExteraDiscountRangeAmountMainGridResultVM>() 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    company = t.CarExteraDiscountRangeAmountCompanies.Select(tt => tt.Company.Title).ToList(),
                    exteraDiscountTitle = t.CarExteraDiscount.Title,
                    title = t.Title,
                    minValue = t.MinValue,
                    maxValue = t.MaxValue,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new CarExteraDiscountRangeAmountMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    exteraDiscountTitle = t.exteraDiscountTitle,
                    title = t.title,
                    minValue = t.minValue == 0 ? "0" : t.minValue.ToString("###,###"),
                    maxValue = t.maxValue.ToString("###,###"),
                    isActive = t.isActive == true    ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarExteraDiscountRangeAmountVM input)
        {
            CreateValidation(input);

            var foundItem = db.CarExteraDiscountRangeAmounts.Where(t => t.Id == input.id).Include(t => t.CarExteraDiscountRangeAmountCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CarExteraDiscountRangeAmountCompanies != null && foundItem.CarExteraDiscountRangeAmountCompanies.Count > 0)
                foreach (var company in foundItem.CarExteraDiscountRangeAmountCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            foundItem.Amount = input.price;
            foundItem.CarExteraDiscountId = input.carExteraDiscountId.Value;
            foundItem.CarExteraDiscountValueId = input.carExteraDiscountValueId;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MaxValue = input.maxValue.Value;
            foundItem.MinValue = input.minValue.Value;
            foundItem.Percent = input.percent;
            foundItem.Title = input.title;
            foundItem.CreateDateSelfPercent = input.cdSelfPercent;

            db.SaveChanges();

            if (input.cIds != null && input.cIds.Count > 0)
                foreach (var cId in input.cIds)
                    db.Entry(new CarExteraDiscountRangeAmountCompany() { CompanyId = cId, CarExteraDiscountRangeAmountId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
