using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormStatusService : IGeneralFormStatusService
    {
        readonly GeneralFormDBContext db = null;
        public GeneralFormStatusService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(GeneralFormStatusCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new GeneralFormStatus()
            {
                GeneralFormId = input.fId.ToLongReturnZiro(),
                Title = input.title,
                Order = input.order.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(GeneralFormStatusCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.fId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

        }

        public ApiResult Delete(long? id)
        {
            var foundItem = db.GeneralFormStatuses.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id)
        {
            return db.GeneralFormStatuses
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    fId = t.GeneralFormId,
                    fId_Title = t.GeneralForm.Title,
                    order = t.Order
                })
                .FirstOrDefault();
        }

        public GridResultVM<GeneralFormStatusMainGridResultVM> GetList(GeneralFormStatusMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GeneralFormStatuses.AsQueryable();

            int row = searchInput.skip;

            return new GridResultVM<GeneralFormStatusMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.GeneralFormId).OrderBy(t => t.Order).OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    fId = t.GeneralForm.Title
                })
                .ToList()
                .Select(t => new GeneralFormStatusMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    fid = t.fId,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(GeneralFormStatusCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.GeneralFormStatuses.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.GeneralFormId = input.fId.ToLongReturnZiro();
            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public long GetFirstId(long generalFormId)
        {
            return db.GeneralFormStatuses.Where(t => t.GeneralFormId == generalFormId).OrderBy(t => t.Order).Select(t => t.Id).FirstOrDefault();
        }
    }
}
