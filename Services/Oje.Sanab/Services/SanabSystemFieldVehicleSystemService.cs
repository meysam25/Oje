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
    public class SanabSystemFieldVehicleSystemService : ISanabSystemFieldVehicleSystemService
    {
        readonly SanabDBContext db = null;
        readonly IVehicleSystemService VehicleSystemService = null;

        public SanabSystemFieldVehicleSystemService
            (
                SanabDBContext db,
                IVehicleSystemService VehicleSystemService
            )
        {
            this.db = db;
            this.VehicleSystemService = VehicleSystemService;
        }

        public ApiResult Create(SanabSystemFieldVehicleSystemCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new SanabSystemFieldVehicleSystem()
            {
                Code = input.code,
                Title = input.title,
                VehicleSystemId = input.vid.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabSystemFieldVehicleSystemCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.code) && input.code.Length > 30)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.vid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if (!VehicleSystemService.Exist(input.vid.Value))
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if(db.SanabSystemFieldVehicleSystems.Any(t => t.Id != input.id && t.Title == input.title))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SanabSystemFieldVehicleSystems.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SanabSystemFieldVehicleSystems
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    vid = t.VehicleSystemId,
                    vid_Title = t.VehicleSystem.Title,
                    code = t.Code
                })
                .FirstOrDefault();
        }

        public GridResultVM<SanabSystemFieldVehicleSystemMainGridResultVM> GetList(SanabSystemFieldVehicleSystemMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SanabSystemFieldVehicleSystems.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.vTitle))
                quiryResult = quiryResult.Where(t => t.VehicleSystem.Title.Contains(searchInput.vTitle));
            if(!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));

            int row = searchInput.skip;

            return new GridResultVM<SanabSystemFieldVehicleSystemMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    title = t.Title,
                    vTitle = t.VehicleSystem.Title,
                    code = t.Code,
                })
                .ToList()
                .Select(t => new SanabSystemFieldVehicleSystemMainGridResultVM 
                {
                    row = ++row,
                    code = t.code,
                    id = t.id,
                    title = t.title,
                    vTitle = t.vTitle
                })
                .ToList()
            };
        }

        public ApiResult Update(SanabSystemFieldVehicleSystemCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SanabSystemFieldVehicleSystems.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.code;
            foundItem.Title = input.title;
            foundItem.VehicleSystemId = input.vid.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetSystemId(string title)
        {
            return db.SanabSystemFieldVehicleSystems.Where(t => t.Title == title).Select(t => t.VehicleSystemId).FirstOrDefault();
        }

        public VehicleSystem GetSystem(string title)
        {
            return db.SanabSystemFieldVehicleSystems.Where(t => t.Title == title).Select(t => t.VehicleSystem).FirstOrDefault();
        }
    }
}
