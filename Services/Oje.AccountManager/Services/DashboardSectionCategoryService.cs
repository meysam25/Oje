using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class DashboardSectionCategoryService : IDashboardSectionCategoryService
    {
        readonly AccountDBContext db = null;
        public DashboardSectionCategoryService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(DashboardSectionCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new DashboardSectionCategory()
            {
                Css = input.cssClass,
                Title = input.title,
                Type = input.type,
                Order = input.order,
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(DashboardSectionCategoryCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (String.IsNullOrEmpty(input.cssClass))
                throw BException.GenerateNewException(BMessages.Please_Enter_Class);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.DashboardSectionCategories.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.DashboardSectionCategories
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    cssClass = t.Css,
                    type = t.Type,
                    order = t.Order
                })
                .FirstOrDefault();
        }

        public GridResultVM<DashboardSectionCategoryServiceMainGridResult> GetList(DashboardSectionCategoryServiceMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new DashboardSectionCategoryServiceMainGrid();

            var qureResult = db.DashboardSectionCategories.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.cssClass))
                qureResult = qureResult.Where(t => t.Css.Contains(searchInput.cssClass));

            int row = searchInput.skip;

            return new GridResultVM<DashboardSectionCategoryServiceMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    cssClass = t.Css
                })
                .ToList()
                .Select(t => new DashboardSectionCategoryServiceMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    cssClass = t.cssClass
                })
                .ToList()
            };
        }

        public ApiResult Update(DashboardSectionCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.DashboardSectionCategories.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Css = input.cssClass;
            foundItem.Type = input.type;
            foundItem.Order = input.order;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.DashboardSectionCategories.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
