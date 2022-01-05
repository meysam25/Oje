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
    public class CarSpecificationService : ICarSpecificationService
    {
        readonly CarDBContext db = null;
        public CarSpecificationService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarSpecificationVM input)
        {
            CreateValidation(input);

            var newitem = new CarSpecification()
            {
                CarRoomRate = input.carRoomRate,
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse()
            };
            db.Entry(newitem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var VehicleSpecId in input.vehicleSpecIds)
                db.Entry(new CarSpecificationVehicleSpec() 
                {
                    CarSpecificationId = newitem.Id,
                    VehicleSpecId = VehicleSpecId
                } ).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateCarSpecificationVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (db.CarSpecifications.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.carRoomRate != null && input.carRoomRate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_CarRomeRate);
            if (input.vehicleSpecIds == null || input.vehicleSpecIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarSpecifications.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.CarSpecifications.Where(t => t.Id == id).Select(t => new 
            {
                id = t.Id,
                title = t.Title,
                carRoomRate = t.CarRoomRate,
                isActive = t.IsActive,
                vehicleSpecIds = t.CarSpecificationVehicleSpecs.Select(tt => new { id = tt.VehicleSpecId, title = tt.VehicleSpec.Title }).ToList()
            }).FirstOrDefault();
        }

        public GridResultVM<CarSpecificationMainGridResultVM> GetList(CarSpecificationMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarSpecificationMainGrid();

            var qureResult = db.CarSpecifications.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.carRomeRate != null && searchInput.carRomeRate > 0)
                qureResult = qureResult.Where(t => t.CarRoomRate == searchInput.carRomeRate);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.specs))
                qureResult = qureResult.Where(t => t.CarSpecificationVehicleSpecs.Any(tt => tt.VehicleSpec.Title.Contains(searchInput.specs)));

            int row = searchInput.skip;

            return new GridResultVM<CarSpecificationMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    carRomeRate = t.CarRoomRate,
                    specs = t.CarSpecificationVehicleSpecs.Select(tt => tt.VehicleSpec.Title).ToList()
                })
                .ToList()
                .Select(t => new CarSpecificationMainGridResultVM
                {
                    carRomeRate = t.carRomeRate,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    row = ++row,
                    title = t.title,
                    specs = string.Join(",", t.specs)
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarSpecificationVM input)
        {
            CreateValidation(input);

            var foundItem = db.CarSpecifications.Include(t => t.CarSpecificationVehicleSpecs).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach (var item in foundItem.CarSpecificationVehicleSpecs)
                db.Entry(item).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.CarRoomRate = input.carRoomRate;

            foreach (var VehicleSpecId in input.vehicleSpecIds)
                db.Entry(new CarSpecificationVehicleSpec()
                {
                    CarSpecificationId = foundItem.Id,
                    VehicleSpecId = VehicleSpecId
                }).State = EntityState.Added;

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

            var qureResult = db.CarSpecifications.OrderByDescending(t => t.Id).AsQueryable();
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
