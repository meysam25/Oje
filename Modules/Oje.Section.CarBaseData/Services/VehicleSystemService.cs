using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleSystemService : IVehicleSystemService
    {
        readonly CarDBContext db = null;
        public VehicleSystemService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateVehicleSystemVM input)
        {
            CreateValidation(input);

            var newItem = new VehicleSystem()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var vtId in input.vehicleTypeIds)
                db.Entry(new VehicleSystemVehicleType() { VehicleTypeId = vtId, VehicleSystemId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateVehicleSystemVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.vehicleTypeIds == null || input.vehicleTypeIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VeicleType);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleSystems.Include(t => t.VehicleSystemVehicleTypes).Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach(var item in foundItem.VehicleSystemVehicleTypes)
                db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateVehicleSystemVM GetById(int? id)
        {
            return db.VehicleSystems.Where(t => t.Id == id)
                .Select(t => new CreateUpdateVehicleSystemVM
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    order = t.Order,
                    title = t.Title,
                    vehicleTypeIds = t.VehicleSystemVehicleTypes.Select(tt => tt.VehicleTypeId).ToList()
                })
                .FirstOrDefault();
        }

        public GridResultVM<VehicleSystemMainGridResultVM> GetList(VehicleSystemMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleSystemMainGrid();

            var qureResult = db.VehicleSystems.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.order != null)
                qureResult = qureResult.Where(t => t.Order == searchInput.order);
            if (searchInput.types.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSystemVehicleTypes.Any(tt => tt.VehicleTypeId == searchInput.types));

            int row = searchInput.skip;

            return new GridResultVM<VehicleSystemMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderBy(t => t.Order).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    order = t.Order,
                    isActive = t.IsActive,
                    types = t.VehicleSystemVehicleTypes.Select(tt => tt.VehicleType.Title).ToList()
                })
                .ToList()
                .Select(t => new VehicleSystemMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    order = t.order,
                    types = string.Join(",", t.types)
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateVehicleSystemVM input)
        {
            CreateValidation(input);

            var foundItem = db.VehicleSystems.Include(t => t.VehicleSystemVehicleTypes).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach (var item in foundItem.VehicleSystemVehicleTypes)
                db.Entry(item).State = EntityState.Deleted;

            foreach (var vtId in input.vehicleTypeIds)
                db.Entry(new VehicleSystemVehicleType() { VehicleTypeId = vtId, VehicleSystemId = foundItem.Id }).State = EntityState.Added;

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? vehicleTypesId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSystems.OrderBy(t => t.Order).AsQueryable();
            if(vehicleTypesId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSystemVehicleTypes.Any(tt => tt.VehicleTypeId == vehicleTypesId));
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
