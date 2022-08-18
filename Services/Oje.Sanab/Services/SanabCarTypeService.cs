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
    public class SanabCarTypeService : ISanabCarTypeService
    {
        readonly SanabDBContext db = null;
        readonly ICarTypeService CarTypeService = null;
        public SanabCarTypeService
            (
                SanabDBContext db,
                ICarTypeService CarTypeService
            )
        {
            this.db = db;
            this.CarTypeService = CarTypeService;
        }

        public ApiResult Create(SanabCarTypeCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new SanabCarType()
            {
                CarTypeId = input.ctId.Value,
                Title = input.title,
                Code = input.code.Value,
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabCarTypeCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.ctId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
            if (!CarTypeService.Exist(input.ctId))
                throw BException.GenerateNewException(BMessages.Please_Select_CarType);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (db.SanabCarTypes.Any(t => t.Code == input.code && t.Id != input.id) || db.SanabCarTypes.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SanabCarTypes.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SanabCarTypes
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    ctId = t.CarTypeId,
                    code = t.Code
                })
                .FirstOrDefault();
        }

        public GridResultVM<SanabCarTypeMainGridResultVM> GetList(SanabCarTypeMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SanabCarTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.ctTitle))
                quiryResult = quiryResult.Where(t => t.CarType.Title.Contains(searchInput.ctTitle));
            if (searchInput.code.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Code == searchInput.code);

            int row = searchInput.skip;

            return new GridResultVM<SanabCarTypeMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    title = t.Title,
                    id = t.Id,
                    ctTitle = t.CarType.Title,
                    code = t.Code
                })
                .ToList()
                .Select(t => new SanabCarTypeMainGridResultVM
                {
                    code = t.code.ToString(),
                    ctTitle = t.ctTitle,
                    id = t.id,
                    row = ++row,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(SanabCarTypeCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SanabCarTypes.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.CarTypeId = input.ctId.Value;
            foundItem.Title = input.title;
            foundItem.Code = input.code.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetTypeIdBy(long? code)
        {
            return db.SanabCarTypes.Where(t => t.Code == code).Select(t => t.CarTypeId).FirstOrDefault();
        }
    }
}
