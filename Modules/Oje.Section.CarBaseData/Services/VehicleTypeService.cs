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
using System.Collections.Generic;

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        readonly CarDBContext db = null;
        public VehicleTypeService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateVehicleTypeVM input)
        {
            CreateValidation(input);

            var newItem = new VehicleType()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                Order = input.order.ToIntReturnZiro(),
                VehicleSpecCategoryId = input.specCatId.ToIntReturnZiro()
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var vtId in input.carTypeIds)
                db.Entry(new VehicleTypeCarType() { CarTypeId = vtId, VehicleTypeId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateVehicleTypeVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.specCatId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecificationCategoryId);
            if (input.carTypeIds == null || input.carTypeIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleTypes.Include(t => t.VehicleTypeCarTypes).Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach (var item in foundItem.VehicleTypeCarTypes)
                db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateVehicleTypeVM GetById(int? id)
        {
            return db.VehicleTypes.Where(t => t.Id == id).Select(t => new CreateUpdateVehicleTypeVM
            {
                id = t.Id,
                isActive = t.IsActive,
                title = t.Title,
                order = t.Order,
                specCatId = t.VehicleSpecCategoryId,
                carTypeIds = t.VehicleTypeCarTypes.Select(tt => tt.CarTypeId).ToList()
            }).FirstOrDefault();
        }

        public GridResultVM<VehicleTypeMainGridResultVM> GetList(VehicleTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleTypeMainGrid();

            var qureResult = db.VehicleTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.specCategory.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSpecCategoryId == searchInput.specCategory);
            if (searchInput.types.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleTypeCarTypes.Any(tt => tt.CarTypeId == searchInput.types));

            int row = searchInput.skip;

            return new GridResultVM<VehicleTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderBy(t => t.Order).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    specCategory = t.VehicleSpecCategory.Title,
                    isActive = t.IsActive,
                    types = t.VehicleTypeCarTypes.Select(tt => tt.CarType.Title).ToList()
                })
                .ToList()
                .Select(t => new VehicleTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    specCategory = t.specCategory,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    title = t.title,
                    types = string.Join(",", t.types)
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateVehicleTypeVM input)
        {
            CreateValidation(input);

            var foundItem = db.VehicleTypes.Include(t => t.VehicleTypeCarTypes).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach (var item in foundItem.VehicleTypeCarTypes)
                db.Entry(item).State = EntityState.Deleted;

            foreach (var vtId in input.carTypeIds)
                db.Entry(new VehicleTypeCarType() { CarTypeId = vtId, VehicleTypeId = foundItem.Id }).State = EntityState.Added;

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.VehicleSpecCategoryId = input.specCatId.ToIntReturnZiro();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleTypes.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
