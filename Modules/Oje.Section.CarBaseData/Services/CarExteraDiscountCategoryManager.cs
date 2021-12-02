using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class CarExteraDiscountCategoryManager : ICarExteraDiscountCategoryManager
    {
        readonly CarDBContext db = null;
        public CarExteraDiscountCategoryManager(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarExteraDiscountCategoryVM input)
        {
            createUpdateValidation(input);

            db.Entry(new CarExteraDiscountCategory()
            {
                Order = input.order.ToIntReturnZiro(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdateCarExteraDiscountCategoryVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (db.CarExteraDiscountCategories.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
        }

        public ApiResult Delete(int? id)
        {
            var deleteItem = db.CarExteraDiscountCategories.Where(t => t.Id == id).FirstOrDefault();
            if (deleteItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            db.Entry(deleteItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarExteraDiscountCategoryVM GetById(int? id)
        {
            return db.CarExteraDiscountCategories
                .Where(t => t.Id == id)
                .AsNoTracking()
                .Select(t => new CreateUpdateCarExteraDiscountCategoryVM
                {
                    id = t.Id,
                    order = t.Order,
                    title = t.Title
                }).FirstOrDefault();
        }

        public GridResultVM<CarExteraDiscountCategoryMainGridResult> GetList(CarExteraDiscountCategoryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarExteraDiscountCategoryMainGrid();

            var qureResult = db.CarExteraDiscountCategories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.order != null)
                qureResult = qureResult.Where(t => t.Order == searchInput.order);

            int row = searchInput.skip;

            return new GridResultVM<CarExteraDiscountCategoryMainGridResult>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Order).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    order = t.Order,
                    title = t.Title
                })
                .ToList()
                .Select(t => new CarExteraDiscountCategoryMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    order = t.order
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarExteraDiscountCategoryVM input)
        {
            createUpdateValidation(input);

            var editItem = db.CarExteraDiscountCategories.Where(t => t.Id == input.id).FirstOrDefault();
            if (editItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            editItem.Order = input.order.ToIntReturnZiro();
            editItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object> { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.CarExteraDiscountCategories.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
