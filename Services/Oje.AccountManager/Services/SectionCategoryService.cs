using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class SectionCategoryService : ISectionCategoryService
    {
        readonly AccountDBContext db = null;
        public SectionCategoryService(AccountDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(SectionCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var newItem = new SectionCategory() { Title = input.title, Order = input.order.ToIntReturnZiro(), IsActive = input.isActive.ToBooleanReturnFalse(), Icon = input.icon };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.sectionIds != null && input.sectionIds.Any())
                foreach (var sectionId in input.sectionIds)
                    db.Entry(new SectionCategorySection()
                    {
                        SectionCategoryId = newItem.Id,
                        SectionId = sectionId
                    }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SectionCategoryCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SectionCategories.Include(t => t.SectionCategorySections).FirstOrDefault(t => t.Id == id);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.SectionCategorySections != null)
                foreach (var item in foundItem.SectionCategorySections)
                    db.Entry(item).State = EntityState.Deleted;
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SectionCategories.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                order = t.Order,
                isActive = t.IsActive,
                icon = t.Icon,
                sectionIds = t.SectionCategorySections.Select(tt => new { id = tt.SectionId, title = tt.Section.Title }).ToList()
            }).FirstOrDefault();
        }

        public GridResultVM<SectionCategoryMainGridResultVM> GetList(SectionCategoryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new SectionCategoryMainGrid();

            var qureResult = db.SectionCategories.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.actionTitle))
                qureResult = qureResult.Where(t => t.SectionCategorySections.Any(tt => tt.Section.Title.Contains(searchInput.actionTitle)));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<SectionCategoryMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    actionTitle = t.SectionCategorySections.Select(tt => tt.Section.Title).ToList(),
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new SectionCategoryMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    actionTitle = string.Join(',', t.actionTitle),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(SectionCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SectionCategories.Include(t => t.SectionCategorySections).FirstOrDefault(t => t.Id == input.id);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var item in foundItem.SectionCategorySections)
                db.Entry(item).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Icon = input.icon;

            if (input.sectionIds != null && input.sectionIds.Any())
                foreach (var sectionId in input.sectionIds)
                    db.Entry(new SectionCategorySection()
                    {
                        SectionCategoryId = foundItem.Id,
                        SectionId = sectionId
                    }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
