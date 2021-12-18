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

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleTypeService: IVehicleTypeService
    {
        readonly CarDBContext db = null;
        public VehicleTypeService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateVehicleTypeVM input)
        {
            CreateValidation(input);

            db.Entry(new VehicleType() 
            {
                CarSpecificationId = input.carSpecId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                VehicleSystemId = input.carVehicleSystemId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateVehicleTypeVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.carSpecId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (input.carVehicleSystemId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (db.VehicleTypes.Any(t => t.Title == input.title && t.VehicleSystemId == input.carVehicleSystemId && t.CarSpecificationId == input.carSpecId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleTypes.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateVehicleTypeVM GetById(int? id)
        {
            return db.VehicleTypes.Where(t => t.Id == id).Select(t => new CreateUpdateVehicleTypeVM
            {
                id = t.Id,
                carSpecId = t.CarSpecificationId,
                carVehicleSystemId = t.VehicleSystemId,
                isActive = t.IsActive,
                title = t.Title,
                carSpecId_Title = t.CarSpecification.Title,
                carVehicleSystemId_Title = t.VehicleSystem.Title
            }).FirstOrDefault();
        }

        public GridResultVM<VehicleTypeMainGridResultVM> GetList(VehicleTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleTypeMainGrid();

            var qureResult = db.VehicleTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.specTitle))
                qureResult = qureResult.Where(t => t.CarSpecification.Title.Contains(searchInput.specTitle));
            if (!string.IsNullOrEmpty(searchInput.brandTitle))
                qureResult = qureResult.Where(t => t.VehicleSystem.Title.Contains(searchInput.brandTitle));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<VehicleTypeMainGridResultVM>() 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    title = t.Title,
                    specTitle = t.CarSpecification.Title,
                    brandTitle = t.VehicleSystem.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new VehicleTypeMainGridResultVM 
                {
                    row = ++row,
                    brandTitle = t.brandTitle,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    specTitle = t.specTitle,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateVehicleTypeVM input)
        {
            CreateValidation(input);

            var foundItem = db.VehicleTypes.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.CarSpecificationId = input.carSpecId.Value;
            foundItem.VehicleSystemId = input.carVehicleSystemId.Value;
            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
