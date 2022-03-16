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
    public class ControllerCategoryService: IControllerCategoryService
    {
        readonly AccountDBContext db = null;
        public ControllerCategoryService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(ControllerCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var newItem = new ControllerCategory() { Title = input.title, Order = input.order.ToIntReturnZiro(), IsActive = input.isActive.ToBooleanReturnFalse(), Icon = input.icon };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.controllerIds != null && input.controllerIds.Any())
                foreach (var cid in input.controllerIds)
                    db.Entry(new ControllerCategoryController()
                    {
                        ControllerId = cid,
                        ControllerCategoryId = newItem.Id
                    }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ControllerCategoryCreateUpdateVM input)
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
            var foundItem = db.ControllerCategories.Include(t => t.ControllerCategoryControllers).FirstOrDefault(t => t.Id == id);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.ControllerCategoryControllers != null)
                foreach (var item in foundItem.ControllerCategoryControllers)
                    db.Entry(item).State = EntityState.Deleted;
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.ControllerCategories.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                order = t.Order,
                isActive = t.IsActive,
                icon = t.Icon,
                controllerIds = t.ControllerCategoryControllers.Select(tt => new { id = tt.ControllerId, title = tt.Controller.Title }).ToList()
            }).FirstOrDefault();
        }

        public GridResultVM<ControllerCategoryMainGridResultVM> GetList(ControllerCategoryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ControllerCategoryMainGrid();

            var qureResult = db.ControllerCategories.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.actionTitle))
                qureResult = qureResult.Where(t => t.ControllerCategoryControllers.Any(tt => tt.Controller.Title.Contains(searchInput.actionTitle)));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ControllerCategoryMainGridResultVM>()
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
                    actionTitle = t.ControllerCategoryControllers.Select(tt => tt.Controller.Title).ToList(),
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ControllerCategoryMainGridResultVM
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

        public ApiResult Update(ControllerCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.ControllerCategories.Include(t => t.ControllerCategoryControllers).FirstOrDefault(t => t.Id == input.id);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var item in foundItem.ControllerCategoryControllers)
                db.Entry(item).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Icon = input.icon;

            if (input.controllerIds != null && input.controllerIds.Any())
                foreach (var cid in input.controllerIds)
                    db.Entry(new ControllerCategoryController()
                    {
                        ControllerCategoryId= foundItem.Id,
                        ControllerId = cid
                    }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
