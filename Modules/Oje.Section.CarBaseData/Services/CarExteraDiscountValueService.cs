using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class CarExteraDiscountValueService : ICarExteraDiscountValueService
    {
        readonly CarDBContext db = null;
        public CarExteraDiscountValueService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarExteraDiscountValueVM input)
        {
            CreateValidation(input);

            db.Entry(new CarExteraDiscountValue()
            {
                CarExteraDiscountId = input.ceId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateCarExteraDiscountValueVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.ceId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarExteraDisocunt);
            if (db.CarExteraDiscountValues.Any(t => t.Title == input.title && t.CarExteraDiscountId == input.ceId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarExteraDiscountValues.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarExteraDiscountValueVM GetById(int? id)
        {
            return db.CarExteraDiscountValues.Where(t => t.Id == id).Where(t => t.Id == id).Select(t => new CreateUpdateCarExteraDiscountValueVM
            {
                id = t.Id,
                title = t.Title,
                ceId = t.CarExteraDiscountId,
                ceId_Title = t.CarExteraDiscount.Title,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<CarExteraDiscountValueMainGridResultVM> GetList(CarExteraDiscountValueMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarExteraDiscountValueMainGrid();

            var qureResult = db.CarExteraDiscountValues.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.exteraDTitle))
                qureResult = qureResult.Where(t => t.CarExteraDiscount.Title.Contains(searchInput.exteraDTitle));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<CarExteraDiscountValueMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    exteraDTitle = t.CarExteraDiscount.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new CarExteraDiscountValueMainGridResultVM
                {
                    row = ++row,
                    exteraDTitle = t.exteraDTitle,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarExteraDiscountValueVM input)
        {
            CreateValidation(input);

            var foundItem = db.CarExteraDiscountValues.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.CarExteraDiscountId = input.ceId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightListByCarExteraDiscountId(int carExteraDiscountId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.CarExteraDiscountValues.Where(t => t.CarExteraDiscountId == carExteraDiscountId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
