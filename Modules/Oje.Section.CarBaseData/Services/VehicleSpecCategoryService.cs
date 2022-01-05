using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.DB;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Services
{
    public class VehicleSpecCategoryService : IVehicleSpecCategoryService
    {
        readonly CarDBContext db = null;
        public VehicleSpecCategoryService(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(VehicleSpecCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new VehicleSpecCategory()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(VehicleSpecCategoryCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.VehicleSpecCategories.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.VehicleSpecCategories.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<VehicleSpecCategoryMainGridResultVM> GetList(VehicleSpecCategoryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new VehicleSpecCategoryMainGrid();

            var qureResult = db.VehicleSpecCategories.AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<VehicleSpecCategoryMainGridResultVM>()
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
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new VehicleSpecCategoryMainGridResultVM
                {
                    id = t.id,
                    row = ++row,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(VehicleSpecCategoryCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.VehicleSpecCategories.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };
            result.AddRange(db.VehicleSpecCategories.Select(t => new { id = t.Id, title = t.Title }).ToList());
            return result;
        }
    }
}
