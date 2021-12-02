using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.BaseData.Services.EContext;

namespace Oje.Section.BaseData.Services
{
    public class JobManager: IJobManager
    {
        readonly BaseDataDBContext db = null;
        public JobManager(BaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateJobVM input)
        {
            CreateValidation(input);

            db.Entry(new Job 
            {
                Title = input.title,
                JobDangerLevelId = input.dlId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateJobVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.dlId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Danger_Level);
            if (db.Jobs.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Jobs.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateJobVM GetById(int? id)
        {
            return db.Jobs.Where(t => t.Id == id).Select(t => new CreateUpdateJobVM 
            {
                dlId = t.JobDangerLevelId,
                title = t.Title,
                isActive = t.IsActive,
                id = t.Id
            }).FirstOrDefault();
        }

        public GridResultVM<JobMainGridResultVM> GetList(JobMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new JobMainGrid();

            var qureResult = db.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.dlId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.JobDangerLevelId == searchInput.dlId);

            var row = searchInput.skip;

            return new GridResultVM<JobMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    dlId = t.JobDangerLevel.Title
                })
                .ToList()
                .Select(t => new JobMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ?BMessages.Active.GetAttribute<DisplayAttribute>()?.Name: BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    dlId = t.dlId
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateJobVM input)
        {
            CreateValidation(input);

            var foundItem = db.Jobs.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.JobDangerLevelId = input.dlId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
