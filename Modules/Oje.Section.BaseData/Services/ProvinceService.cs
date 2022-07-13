using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.BaseData.Services.EContext;

namespace Oje.Section.BaseData.Services
{
    public class ProvinceService : IProvinceService
    {
        readonly BaseDataDBContext db = null;
        public ProvinceService(BaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateProvinceVM input)
        {
            CreateValidation(input);

            db.Entry(new Province()
            {
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MapZoom = input.mapZoom,
                MapLon = input.mapLon,
                MapLat = input.mapLat,
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateProvinceVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (db.Provinces.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Provinces.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateProvinceVM GetById(int? id)
        {
            return db.Provinces.Where(t => t.Id == id).Select(t => new CreateUpdateProvinceVM
            {
                id = t.Id,
                title = t.Title,
                isActive = t.IsActive,
                mapLat = t.MapLat,
                mapLon = t.MapLon,
                mapZoom = t.MapZoom
            }).FirstOrDefault();
        }

        public GridResultVM<ProvinceMainGridResultVM> GetList(ProvinceMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProvinceMainGrid();

            var qureResult = db.Provinces.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.cityCount != null && searchInput.cityCount >= 0)
                qureResult = qureResult.Where(t => t.Cities.Count == searchInput.cityCount);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<ProvinceMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    cityCount = t.Cities.Count()
                })
                .ToList()
                .Select(t => new ProvinceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    cityCount = t.cityCount
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProvinceVM input)
        {
            CreateValidation(input);

            var foundItem = db.Provinces.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MapZoom = input.mapZoom;
            foundItem.MapLon = input.mapLon;
            foundItem.MapLat = input.mapLat;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Provinces.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
