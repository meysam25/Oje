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
    public class JobDangerLevelManager : IJobDangerLevelManager
    {
        readonly BaseDataDBContext db = null;
        public JobDangerLevelManager(BaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateJobDangerLevelVM input)
        {
            CreateValidation(input);

            db.Entry(new JobDangerLevel()
            {
                Title = input.title,
                DangerLevel = input.danger.Value,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateJobDangerLevelVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.danger == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Danger_Level, ApiResultErrorCode.ValidationError);
            if (input.danger < 0)
                throw BException.GenerateNewException(BMessages.Danger_Level_Can_Not_Be_Less_Then_0, ApiResultErrorCode.ValidationError);
            if (db.JobDangerLevels.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.JobDangerLevels.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateJobDangerLevelVM GetById(int? id)
        {
            return
                db.JobDangerLevels.Where(t => t.Id == id).AsNoTracking().Select(t => new CreateUpdateJobDangerLevelVM
                {
                    id = t.Id,
                    title = t.Title,
                    danger = t.DangerLevel,
                    isActive = t.IsActive
                }).FirstOrDefault();
        }

        public GridResultVM<JobDangerLevelMainGridResultVM> GetList(JobDangerLevelMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new JobDangerLevelMainGrid();

            var qureResult = db.JobDangerLevels.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.danger != null && searchInput.danger >= 0)
                qureResult = qureResult.Where(t => t.DangerLevel == searchInput.danger);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<JobDangerLevelMainGridResultVM> 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    danger = t.DangerLevel
                })
                .ToList()
                .Select(t => new JobDangerLevelMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    danger = t.danger
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateJobDangerLevelVM input)
        {
            CreateValidation(input);

            var foundItem = db.JobDangerLevels.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.DangerLevel = input.danger.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new List<object> { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.JobDangerLevels.AsNoTracking().Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
