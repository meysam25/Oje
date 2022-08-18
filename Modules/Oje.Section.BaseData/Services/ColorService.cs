using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.DB;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.BaseData.Services
{
    public class ColorService: IColorService
    {
        readonly BaseDataDBContext db = null;
        public ColorService(BaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(ColorCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new Color() 
            {
                Title = input.title,
                Code = input.code,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ColorCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.code.Length > 50)
                throw BException.GenerateNewException(BMessages.String_Length_Is_Not_Acceptable);
            if (db.Colors.Any(t => t.Id != input.id && t.Title == input.title))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Colors.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.Colors
                .Where(t => t.Id == id)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    code = t.Code,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<ColorMainGridResultVM> GetList(ColorMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.Colors.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ColorMainGridResultVM>()
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
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ColorMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true  ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(ColorCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.Colors.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Code = input.code;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
