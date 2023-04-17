using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class SanabVehicleTypeService : ISanabVehicleTypeService
    {
        readonly SanabDBContext db = null;
        readonly IVehicleTypeService VehicleTypeService = null;

        public SanabVehicleTypeService
            (
                SanabDBContext db,
                IVehicleTypeService VehicleTypeService
            )
        {
            this.db = db;
            this.VehicleTypeService = VehicleTypeService;
        }

        public ApiResult Create(SanabVehicleTypeCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new SanabVehicleType()
            {
                Title = input.title,
                Code = input.code.Value,
                VehicleTypeId = input.vtId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabVehicleTypeCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.vtId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VeicleType);
            if (!VehicleTypeService.Exist(input.vtId.ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_VeicleType);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (db.SanabVehicleTypes.Any(t => t.VehicleTypeId == input.vtId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SanabVehicleTypes.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SanabVehicleTypes
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    code = t.Code,
                    vtId = t.VehicleTypeId,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<SanabVehicleTypeMainGridResultVM> GetList(SanabVehicleTypeMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SanabVehicleTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.vtTitle))
                quiryResult = quiryResult.Where(t => t.VehicleType.Title.Contains(searchInput.vtTitle));
            if (searchInput.code.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Code == searchInput.code);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<SanabVehicleTypeMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    vTitle = t.VehicleType.Title,
                    t.Code,
                    t.Title,
                })
                .ToList()
                .Select(t => new SanabVehicleTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    code = t.Code.ToString(),
                    vtTitle = t.vTitle,
                    title = t.Title
                })
                .ToList()
            };
        }

        public ApiResult Update(SanabVehicleTypeCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SanabVehicleTypes.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.code.Value;
            foundItem.VehicleTypeId = input.vtId.Value;
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetTypeIdBy(int code)
        {
            return db.SanabVehicleTypes.Where(t => t.Code == code).Select(t => t.VehicleTypeId).FirstOrDefault();
        }
    }
}
