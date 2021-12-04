using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Models.DB;
using Oje.Section.CarBodyBaseData.Models.View;
using Oje.Section.CarBodyBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.CarBodyBaseData.Services
{
    public class CarBodyCreateDatePercentManager: ICarBodyCreateDatePercentManager
    {
        readonly CarBodyDBContext db = null;
        public CarBodyCreateDatePercentManager(CarBodyDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarBodyCreateDatePercentVM input)
        {
            createValidation(input);

            db.Entry(new CarBodyCreateDatePercent()
            {
                FromYear = input.fromYear.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                Title = input.title,
                ToYear = input.toYear.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateCarBodyCreateDatePercentVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.fromYear.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_FromYear);
            if (input.toYear.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_ToYear);
            if (input.fromYear >= input.toYear)
                throw BException.GenerateNewException(BMessages.FromYear_Should_Be_Greader_Then_FromYear);
            if (input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.percent <= 0 || input.percent > 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarBodyCreateDatePercents.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarBodyCreateDatePercentVM GetById(int? id)
        {
            return db.CarBodyCreateDatePercents.Where(t => t.Id == id).Select(t => new CreateUpdateCarBodyCreateDatePercentVM
            {
                id = t.Id,
                fromYear = t.FromYear,
                isActive = t.IsActive,
                percent = t.Percent,
                title = t.Title,
                toYear = t.ToYear
            }).FirstOrDefault();
        }

        public GridResultVM<CarBodyCreateDatePercentMainGridResultVM> GetList(CarBodyCreateDatePercentMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarBodyCreateDatePercentMainGrid();

            var qureResult = db.CarBodyCreateDatePercents.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.fromYear.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FromYear == searchInput.fromYear);
            if (searchInput.toYear.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ToYear == searchInput.toYear);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.percent != null && searchInput.percent > 0 && searchInput.percent <= 100)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);

            int row = searchInput.skip;

            return new GridResultVM<CarBodyCreateDatePercentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    fromYear = t.FromYear,
                    toYear = t.ToYear,
                    percent = t.Percent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new CarBodyCreateDatePercentMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    fromYear = t.fromYear,
                    percent = t.percent,
                    title = t.title,
                    toYear = t.toYear,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarBodyCreateDatePercentVM input)
        {
            createValidation(input);

            var foundItem = db.CarBodyCreateDatePercents.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.FromYear = input.fromYear.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.Title = input.title;
            foundItem.ToYear = input.toYear.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
