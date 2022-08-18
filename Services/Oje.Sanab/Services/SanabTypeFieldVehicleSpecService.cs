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
    public class SanabTypeFieldVehicleSpecService : ISanabTypeFieldVehicleSpecService
    {
        readonly SanabDBContext db = null;
        readonly IVehicleSpecService VehicleSpecService = null;
        readonly IVehicleSystemService VehicleSystemService = null;

        public SanabTypeFieldVehicleSpecService
            (
                SanabDBContext db,
                IVehicleSpecService VehicleSpecService,
                IVehicleSystemService VehicleSystemService
            )
        {
            this.db = db;
            this.VehicleSpecService = VehicleSpecService;
            this.VehicleSystemService = VehicleSystemService;
        }

        public ApiResult Create(SanabTypeFieldVehicleSpecCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new SanabTypeFieldVehicleSpec()
            {
                Code = input.Code,
                Title = input.title,
                VehicleSpecId = input.vsId.Value,
                VehicleSystemId = input.vId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabTypeFieldVehicleSpecCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.vId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if (!VehicleSystemService.Exist(input.vId.ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_VehicleSystem);
            if (input.vsId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (!VehicleSpecService.Exist(input.vId.ToIntReturnZiro(), input.vsId.ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (db.SanabTypeFieldVehicleSpecs.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SanabTypeFieldVehicleSpecs.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SanabTypeFieldVehicleSpecs.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                vId = t.VehicleSystemId,
                vId_Title = t.VehicleSystem.Title,
                vsId = t.VehicleSpecId,
                vsId_Title = t.VehicleSpec.Title,
                code = t.Code
            }).FirstOrDefault();
        }

        public GridResultVM<SanabTypeFieldVehicleSpecMainGridResultVM> GetList(SanabTypeFieldVehicleSpecMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SanabTypeFieldVehicleSpecs.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.vTitle))
                quiryResult = quiryResult.Where(t => t.VehicleSystem.Title.Contains(searchInput.vTitle));
            if (!string.IsNullOrEmpty(searchInput.vSTitle))
                quiryResult = quiryResult.Where(t => t.VehicleSpec.Title.Contains(searchInput.vSTitle));
            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));


            int row = searchInput.skip;

            return new GridResultVM<SanabTypeFieldVehicleSpecMainGridResultVM>()
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
                    code = t.Code,
                    vTitle = t.VehicleSystem.Title,
                    vSTitle = t.VehicleSpec.Title
                })
                .ToList()
                .Select(t => new SanabTypeFieldVehicleSpecMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    code = t.code,
                    title = t.title,
                    vSTitle = t.vSTitle,
                    vTitle = t.vTitle
                })
                .ToList()
            };
        }

        public object Update(SanabTypeFieldVehicleSpecCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SanabTypeFieldVehicleSpecs.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.Code;
            foundItem.Title = input.title;
            foundItem.VehicleSpecId = input.vsId.Value;
            foundItem.VehicleSystemId = input.vId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetSpecId(string title)
        {
            return db.SanabTypeFieldVehicleSpecs.Where(t => t.Title == title).Select(t => t.VehicleSpecId).FirstOrDefault();
        }

        public VehicleSpec GetSpec(string title)
        {
            return db.SanabTypeFieldVehicleSpecs.Where(t => t.Title == title).Select(t => t.VehicleSpec).FirstOrDefault();
        }
    }
}
