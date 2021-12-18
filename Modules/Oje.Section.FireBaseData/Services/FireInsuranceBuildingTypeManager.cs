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
    public class FireInsuranceBuildingTypeService: IFireInsuranceBuildingTypeService
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceBuildingTypeService(
                FireBaseDataDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceBuildingTypeVM input)
        {
            createValidation(input);

            db.Entry(new FireInsuranceBuildingType() { Title = input.title, IsActive = input.isActive.ToBooleanReturnFalse() }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceBuildingTypeVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (db.FireInsuranceBuildingTypes.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceBuildingTypes.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceBuildingTypeVM GetById(int? id)
        {
            return db.FireInsuranceBuildingTypes.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceBuildingTypeVM
            {
                id = t.Id,
                title = t.Title,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceBuildingTypeMainGridResultVM> GetList(FireInsuranceBuildingTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceBuildingTypeMainGrid();

            var qureResult = db.FireInsuranceBuildingTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<FireInsuranceBuildingTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceBuildingTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceBuildingTypeVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceBuildingTypes.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.FireInsuranceBuildingTypes.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
