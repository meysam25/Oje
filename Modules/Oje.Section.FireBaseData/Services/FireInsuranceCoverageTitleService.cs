using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Oje.Section.FireBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.FireBaseData.Services.EContext;

namespace Oje.Section.FireBaseData.Services
{
    public class FireInsuranceCoverageTitleService : IFireInsuranceCoverageTitleService
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceCoverageTitleService(
            FireBaseDataDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceCoverageTitleVM input)
        {
            CreateValidation(input);

            db.Entry(new FireInsuranceCoverageTitle()
            {
                Description = input.description,
                EffectOn = input.effect.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateFireInsuranceCoverageTitleVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.effect == null)
                throw BException.GenerateNewException(BMessages.Please_Select_CalcType);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (db.FireInsuranceCoverageTitles.Any(t => t.Title == input.title && t.EffectOn == input.effect && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceCoverageTitles.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceCoverageTitleVM GetById(int? id)
        {
            return db.FireInsuranceCoverageTitles.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceCoverageTitleVM
            {
                id = t.Id,
                description = t.Description,
                effect = t.EffectOn,
                isActive = t.IsActive,
                title = t.Title
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceCoverageTitleMainGridResultVM> GetList(FireInsuranceCoverageTitleMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceCoverageTitleMainGrid();

            var qureResult = db.FireInsuranceCoverageTitles.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.effect != null)
                qureResult = qureResult.Where(t => t.EffectOn == searchInput.effect);

            int row = searchInput.skip;

            return new GridResultVM<FireInsuranceCoverageTitleMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    effect = t.EffectOn
                })
                .ToList()
                .Select(t => new FireInsuranceCoverageTitleMainGridResultVM
                {
                    id = t.id,
                    effect = t.effect.GetAttribute<DisplayAttribute>()?.Name,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    row = ++row,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceCoverageTitleVM input)
        {
            CreateValidation(input);

            var foundItem = db.FireInsuranceCoverageTitles.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Description = input.description;
            foundItem.EffectOn = input.effect.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.FireInsuranceCoverageTitles.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
