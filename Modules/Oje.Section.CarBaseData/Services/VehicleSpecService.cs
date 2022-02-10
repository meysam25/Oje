using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.DB;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleSpecService : IVehicleSpecService
    {
        readonly CarDBContext db = null;
        public VehicleSpecService(
            CarDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(VehicleSpecCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new VehicleSpec()
            {
                Title = input.title,
                VehicleSpecCategoryId = input.specCat.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                VehicleSystemId = input.vSystemId.ToIntReturnZiro(),
                VehicleTypeId = input.vehicleTypeId.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(VehicleSpecCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.specCat.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (input.vSystemId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if (input.vehicleTypeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VeicleType);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleSpecs.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.VehicleSpecs.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                specCat = t.VehicleSpecCategoryId,
                isActive = t.IsActive,
                order = t.Order,
                vSystemId = t.VehicleSystemId,
                vSystemId_Title = t.VehicleSystem.Title,
                vehicleTypeId = t.VehicleTypeId
            }).FirstOrDefault();
        }

        public GridResultVM<VehicleSpecMainGridResultVM> GetList(VehicleSpecMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleSpecMainGrid();

            var qureResult = db.VehicleSpecs.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.specCat.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSpecCategoryId == searchInput.specCat);
            if (!string.IsNullOrEmpty(searchInput.vSystem))
                qureResult = qureResult.Where(t => t.VehicleSystem.Title.Contains(searchInput.vSystem));
            if(searchInput.vehicleTypeId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleTypeId == searchInput.vehicleTypeId);

            int row = searchInput.skip;

            return new GridResultVM<VehicleSpecMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    specCat = t.VehicleSpecCategory.Title,
                    title = t.Title,
                    vSystem = t.VehicleSystem.Title,
                    vehicleTypeId = t.VehicleType.Title
                })
                .ToList()
                .Select(t => new VehicleSpecMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    specCat = t.specCat,
                    title = t.title,
                    vSystem = t.vSystem,
                    vehicleTypeId = t.vehicleTypeId
                })
                .ToList()
            };
        }

        public ApiResult Update(VehicleSpecCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.VehicleSpecs.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.VehicleSpecCategoryId = input.specCat.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.VehicleSystemId = input.vSystemId.ToIntReturnZiro();
            foundItem.VehicleTypeId = input.vehicleTypeId.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? VehicleSpecCategoryId, int? VehicleSystemId, Select2SearchVM searchInput, int? vehicleTypeId, bool? determine)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSpecs.OrderBy(t => t.Order).AsQueryable();
            if(vehicleTypeId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleTypeId == vehicleTypeId);
            if (determine == true)
                qureResult = qureResult.Where(t => t.CarSpecificationVehicleSpecs.Count > 0);
            if (determine == false)
                qureResult = qureResult.Where(t => t.CarSpecificationVehicleSpecs.Count == 0);
            if (VehicleSpecCategoryId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSpecCategoryId == VehicleSpecCategoryId);
            if (VehicleSystemId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.VehicleSystemId == VehicleSystemId);
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
