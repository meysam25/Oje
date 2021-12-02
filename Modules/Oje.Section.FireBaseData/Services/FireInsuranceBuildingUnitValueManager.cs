using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Oje.Section.FireBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.FireBaseData.Services.EContext;

namespace Oje.Section.FireBaseData.Services
{
    public class FireInsuranceBuildingUnitValueManager : IFireInsuranceBuildingUnitValueManager
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceBuildingUnitValueManager(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceBuildingUnitValueVM input)
        {
            createValidation(input);

            db.Entry(new FireInsuranceBuildingUnitValue()
            {
                Title = input.title,
                Value = input.value.Value,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceBuildingUnitValueVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (db.FireInsuranceBuildingUnitValues.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.value.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Value2);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceBuildingUnitValues.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceBuildingUnitValueVM GetById(int? id)
        {
            return db.FireInsuranceBuildingUnitValues.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceBuildingUnitValueVM
            {
                id = t.Id,
                title = t.Title,
                value = t.Value,
                isActive = t.IsActive
            }).FirstOrDefault();
        }


        public GridResultVM<FireInsuranceBuildingUnitValueMainGridResultVM> GetList(FireInsuranceBuildingUnitValueMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceBuildingUnitValueMainGrid();

            var qureResult = db.FireInsuranceBuildingUnitValues.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.value.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Value == searchInput.value);

            var row = searchInput.skip;

            return new GridResultVM<FireInsuranceBuildingUnitValueMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    value = t.Value,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceBuildingUnitValueMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    value = t.value.ToString("###,###"),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceBuildingUnitValueVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceBuildingUnitValues.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Value = input.value.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.FireInsuranceBuildingUnitValues.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
