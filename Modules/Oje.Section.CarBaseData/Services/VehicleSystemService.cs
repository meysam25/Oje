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

            db.Entry(new VehicleSystem()
            {
                CarTypeId = input.carTypeId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateVehicleSystemVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.carTypeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (db.VehicleSystems.Any(t => t.Title == input.title && t.CarTypeId == input.carTypeId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleSystems.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

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
                    carTypeId = t.CarTypeId,
                    isActive = t.IsActive,
                    order = t.Order,
                    title = t.Title
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
            if (searchInput.carTypeId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CarTypeId == searchInput.carTypeId);

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
                    carTypeId = t.CarType.Title
                })
                .ToList()
                .Select(t => new VehicleSystemMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ?BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    order = t.order,
                    carTypeId = t.carTypeId
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateVehicleSystemVM input)
        {
            CreateValidation(input);

            var foundItem = db.VehicleSystems.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.CarTypeId = input.carTypeId.Value;
            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSystems.OrderByDescending(t => t.Id).AsQueryable();
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
