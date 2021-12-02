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
    public class FireInsuranceTypeOfActivityManager: IFireInsuranceTypeOfActivityManager
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceTypeOfActivityManager(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            createValidation(input);

            db.Entry(new FireInsuranceTypeOfActivity()
            {
                Title = input.title,
                DangerStep = input.danger.Value,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (db.FireInsuranceTypeOfActivities.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.danger.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_DangerLevel);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceTypeOfActivities.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceTypeOfActivityVM GetById(int? id)
        {
            return db.FireInsuranceTypeOfActivities.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceTypeOfActivityVM
            {
                id = t.Id,
                title = t.Title,
                danger = t.DangerStep,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceTypeOfActivityMainGridResultVM> GetList(FireInsuranceTypeOfActivityMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceTypeOfActivityMainGrid();

            var qureResult = db.FireInsuranceTypeOfActivities.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.danger.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.DangerStep == searchInput.danger);

            var row = searchInput.skip;

            return new GridResultVM<FireInsuranceTypeOfActivityMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    danger = t.DangerStep,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceTypeOfActivityMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    danger = t.danger,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceTypeOfActivities.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.DangerStep = input.danger.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
