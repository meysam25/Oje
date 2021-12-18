using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oje.Section.BaseData.Services.EContext;

namespace Oje.Section.BaseData.Services
{
    public class CityService : ICityService
    {
        readonly BaseDataDBContext db = null;
        public CityService(BaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCityVM input)
        {
            CreateValidation(input);

            db.Entry(new City()
            {
                Title = input.title,
                ProvinceId = input.provinceId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                FireDangerGroupLevel = input.fireLevel
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateCityVM input)
        {
            if (input == null)
                BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.provinceId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Province, ApiResultErrorCode.ValidationError);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars, ApiResultErrorCode.ValidationError);
            if (db.Cities.Any(t => t.ProvinceId == input.provinceId && t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Cities.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.Cities.Where(t => t.Id == id).AsNoTracking().Select(t => new 
            {
                id = t.Id,
                title = t.Title,
                provinceId = t.ProvinceId,
                isActive = t.IsActive,
                fireLevel =(int?) t.FireDangerGroupLevel 
            }).FirstOrDefault();
        }

        public GridResultVM<CityMainGriResultVM> GetList(CityMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CityMainGrid();

            var qureResult = db.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.province.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProvinceId == searchInput.province);

            var row = searchInput.skip;

            return new GridResultVM<CityMainGriResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    province = t.Province.Title
                })
                .ToList()
                .Select(t => new CityMainGriResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    province = t.province
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCityVM input)
        {
            CreateValidation(input);

            var foundItem = db.Cities.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.ProvinceId = input.provinceId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.FireDangerGroupLevel = input.fireLevel;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
