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
    public class FireInsuranceCoverageActivityDangerLevelService : IFireInsuranceCoverageActivityDangerLevelService
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceCoverageActivityDangerLevelService(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input)
        {
            createValidation(input);

            db.Entry(new FireInsuranceCoverageActivityDangerLevel()
            {
                FireInsuranceCoverageTitleId = input.coverId.Value,
                Rate = input.rate.Value,
                DangerStep = input.danger.Value,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.coverId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Cover);
            if (input.danger.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_DangerLevel);
            if (input.rate == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate);
            if (input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceCoverageActivityDangerLevels.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceCoverageActivityDangerLevelVM GetById(int? id)
        {
            return db.FireInsuranceCoverageActivityDangerLevels.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceCoverageActivityDangerLevelVM
            {
                id = t.Id,
                danger = t.DangerStep,
                isActive = t.IsActive,
                rate = t.Rate,
                coverId = t.FireInsuranceCoverageTitleId,
                coverId_Title = t.FireInsuranceCoverageTitle.Title
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceCoverageActivityDangerLevelMainGridResultVM> GetList(FireInsuranceCoverageActivityDangerLevelMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceCoverageActivityDangerLevelMainGrid();

            var qureResult = db.FireInsuranceCoverageActivityDangerLevels.AsQueryable();

            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.danger.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.DangerStep == searchInput.danger);
            if (searchInput.rate != null && searchInput.rate > 0)
                qureResult = qureResult.Where(t => t.Rate == searchInput.rate);
            if (!string.IsNullOrEmpty(searchInput.cover))
                qureResult = qureResult.Where(t => t.FireInsuranceCoverageTitle.Title.Contains(searchInput.cover));

            var row = searchInput.skip;

            return new GridResultVM<FireInsuranceCoverageActivityDangerLevelMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    rate = t.Rate,
                    cover = t.FireInsuranceCoverageTitle.Title,
                    danger = t.DangerStep,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceCoverageActivityDangerLevelMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    rate = t.rate,
                    cover = t.cover,
                    danger = t.danger,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceCoverageActivityDangerLevels.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Rate = input.rate.Value;
            foundItem.FireInsuranceCoverageTitleId = input.coverId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.DangerStep = input.danger.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
