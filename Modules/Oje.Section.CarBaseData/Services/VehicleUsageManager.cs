using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleUsageService: IVehicleUsageService
    {
        readonly CarDBContext db = null;
        public VehicleUsageService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateVehicleUsageVM input)
        {
            CreateValidation(input);

            var newItem = new VehicleUsage() 
            {
                BodyPercent = input.bodyPercent,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ThirdPartyPercent = input.thirdPartyPercent,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var typeId in input.carTypeIds)
                db.Entry(new VehicleUsageCarType() { CarTypeId = typeId, VehicleUsageId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateVehicleUsageVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.carTypeIds == null || input.carTypeIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleUsages.Include(t => t.VehicleUsageCarTypes).Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateVehicleUsageVM GetById(int? id)
        {
            return db.VehicleUsages
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateVehicleUsageVM
                { 
                    id = t.Id,
                    bodyPercent = t.BodyPercent,
                    carTypeIds = t.VehicleUsageCarTypes.Select(tt => tt.CarTypeId).ToList(),
                    isActive = t.IsActive,
                    thirdPartyPercent = t.ThirdPartyPercent,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<VehicleUsageMainGridResultVM> GetList(VehicleUsageMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleUsageMainGrid();

            var qureResult = db.VehicleUsages.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.bodyPercent != null && searchInput.bodyPercent > 0)
                qureResult = qureResult.Where(t => t.BodyPercent == searchInput.bodyPercent);
            if (searchInput.thirdPercent != null && searchInput.thirdPercent > 0)
                qureResult = qureResult.Where(t => t.ThirdPartyPercent == searchInput.thirdPercent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.carTypes.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleUsageCarTypes.Any(tt => tt.CarTypeId == searchInput.carTypes));

            int row = searchInput.skip;

            return new GridResultVM<VehicleUsageMainGridResultVM>() 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    carTypes = t.VehicleUsageCarTypes.Select(tt => tt.CarType.Title).ToList(),
                    bodyPercent  = t.BodyPercent,
                    thirdPercent = t.ThirdPartyPercent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new VehicleUsageMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    bodyPercent = t.bodyPercent + "",
                    carTypes = string.Join(",", t.carTypes),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    thirdPercent = t.thirdPercent + "",
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateVehicleUsageVM input)
        {
            CreateValidation(input);

            var foundItem = db.VehicleUsages.Include(t => t.VehicleUsageCarTypes).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.VehicleUsageCarTypes != null)
                foreach (var carType in foundItem.VehicleUsageCarTypes)
                    db.Entry(carType).State = EntityState.Deleted;

            foundItem.ThirdPartyPercent = input.thirdPartyPercent;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.BodyPercent = input.bodyPercent;
            foundItem.Title = input.title;

            foreach (var typeId in input.carTypeIds)
                db.Entry(new VehicleUsageCarType() { CarTypeId = typeId, VehicleUsageId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
