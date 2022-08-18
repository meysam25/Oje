using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class HolydayService : IHolydayService
    {
        readonly AccountDBContext db = null;
        public HolydayService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(HolydayCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new Holyday()
            {
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                TargetDate = input.targetDate.ToEnDate().Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(HolydayCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.targetDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.targetDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            var enTargetDate = input.targetDate.ToEnDate().Value;
            if (db.Holydays.Any(t => t.Id != input.id && t.TargetDate.Year == enTargetDate.Year && t.TargetDate.Month == enTargetDate.Month && t.TargetDate.Day == enTargetDate.Day))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(long? id)
        {
            var foundItem = db.Holydays.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id)
        {
            return db.Holydays
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    targetDate = t.TargetDate,
                    description = t.Description,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    targetDate = t.targetDate.ToFaDate(),
                    t.description,
                    t.isActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<HolydayMainGridResultVM> GetList(HolydayMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.Holydays.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.targetDate) && searchInput.targetDate.ToEnDate() != null)
            {
                var targetDate = searchInput.targetDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.TargetDate.Year == targetDate.Hour && t.TargetDate.Month == targetDate.Month && t.TargetDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<HolydayMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    targetDate = t.TargetDate
                })
                .ToList()
                .Select(t => new HolydayMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    targetDate = t.targetDate.ToFaDate()
                })
                .ToList()
            };
        }

        public ApiResult Update(HolydayCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.Holydays.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.TargetDate = input.targetDate.ToEnDate().Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool IsHolyday(DateTime now)
        {
            if (now.DayOfWeek == DayOfWeek.Friday)
                return true;

            return db.Holydays
                .Where(t => t.IsActive == true && t.TargetDate.Year == now.Year && t.TargetDate.Month == now.Month && t.TargetDate.Day == now.Day)
                .Any();
        }
    }
}
