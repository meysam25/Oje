using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.BaseData.Services.EContext;

namespace Oje.Section.BaseData.Services
{
    public class DutyService : IDutyService
    {
        readonly BaseDataDBContext db = null;
        public DutyService(
                BaseDataDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateDutyVM input)
        {
            CreateValidation(input);

            db.Entry(new Duty()
            {
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                Year = input.year.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateDutyVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.year == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Year);
            if (input.year.ToIntReturnZiro() <= 1000 || input.year.ToIntReturnZiro() >= 2000)
                throw BException.GenerateNewException(BMessages.Year_Is_Invalid);
            if (input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.percent.ToByteReturnZiro() < 0 || input.percent.ToByteReturnZiro() > 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (db.Duties.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (db.Duties.Any(t => t.Year == input.year && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Year);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Duties.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateDutyVM GetById(int? id)
        {
            return db.Duties.AsNoTracking().Where(t => t.Id == id)
                .Select(t => new CreateUpdateDutyVM
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    year = t.Year
                })
                .FirstOrDefault();
        }

        public GridResultVM<DutyMainGridResultVM> GetList(DutyMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new DutyMainGrid();

            var qureResult = db.Duties.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.percent != null && searchInput.percent > 0 && searchInput.percent <= 100)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.year.ToIntReturnZiro() > 1000 && searchInput.year < 2000)
                qureResult = qureResult.Where(t => t.Year == searchInput.year);

            var row = searchInput.skip;

            return new GridResultVM<DutyMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    year = t.Year,
                    isActive = t.IsActive,
                    percent = t.Percent
                })
                .ToList()
                .Select(t => new DutyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    year = t.year,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    percent = t.percent,
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateDutyVM input)
        {
            CreateValidation(input);

            var foundItem = db.Duties.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.Year = input.year.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
