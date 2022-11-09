using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormStatusService : IGeneralFormStatusService
    {
        readonly GeneralFormDBContext db = null;
        readonly IGeneralFormStatusRoleService GeneralFormStatusRoleService = null;

        public GeneralFormStatusService
            (
                GeneralFormDBContext db,
                IGeneralFormStatusRoleService GeneralFormStatusRoleService
            )
        {
            this.db = db;
            this.GeneralFormStatusRoleService = GeneralFormStatusRoleService;
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
                .OrderByDescending(t => t.GeneralFormId).OrderBy(t => t.Order)
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

        public object GetSelect2List(Select2SearchVM searchInput, long? generalFormId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.GeneralFormStatuses.OrderBy(t => t.Order).Where(t => t.GeneralFormId == generalFormId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public List<IdTitle> GetLightList(List<string> roleNames)
        {
            List<IdTitle> result = new();

            if (roleNames != null && roleNames.Count > 0)
            {
                var allUserFormStatus = GeneralFormStatusRoleService.GetByRoles(roleNames);

                if (allUserFormStatus.Count > 0)
                {
                    var allNullStatus = allUserFormStatus.Where(t => t.GeneralFormStatusId == null).ToList();
                    if (allNullStatus != null && allNullStatus.Count > 0)
                    {
                        var allFormIds = allNullStatus.Select(t => t.GeneralFormId).ToList();
                        result.AddRange(

                                db.GeneralFormStatuses
                                .Where(t => allFormIds.Contains(t.GeneralFormId))
                                .Select(t => new IdTitle
                                {
                                    id = t.GeneralFormId + "_" + t.Id,
                                    title = t.GeneralForm.Title + " " + t.Title
                                })
                                .ToList()

                            );
                    }
                    var allWithStatus = allUserFormStatus.Where(t => t.GeneralFormStatusId != null).ToList();
                    if (allWithStatus != null && allWithStatus.Count > 0)
                    {
                        var allFormIds = allWithStatus.Where(t => !allNullStatus.Select(tt => tt.GeneralFormId).Contains(t.GeneralFormId)).Select(t => t.GeneralFormId).ToList();
                        var allStatusIds = allWithStatus.Where(t => !allNullStatus.Select(tt => tt.GeneralFormId).Contains(t.GeneralFormId)).Select(t => t.GeneralFormStatusId).ToList();
                        result.AddRange(

                                db.GeneralFormStatuses
                                .Where(t => allFormIds.Contains(t.GeneralFormId) && allStatusIds.Contains(t.Id))
                                .Select(t => new IdTitle
                                {
                                    id = t.GeneralFormId + "_" + t.Id,
                                    title = t.GeneralForm.Title + " " + t.Title
                                })
                                .ToList()

                            );
                    }
                }

            }

            return result;
        }

        public List<IdTitle> GetNextStatuses(long id)
        {
            List<IdTitle> result = new();

            var foundCurStatus = db.GeneralFormStatuses.Where(t => t.Id == id).FirstOrDefault();
            if (foundCurStatus != null)
            {
                var groupItem = db.GeneralFormStatuses
                    .Where(t => t.GeneralFormId == foundCurStatus.GeneralFormId && t.Order > foundCurStatus.Order)
                    .GroupBy(t => t.Order)
                    .OrderBy(t => t.Key)
                    .Select(t => new { allItems = t.ToList() })
                    .FirstOrDefault();

                if (groupItem != null && groupItem.allItems != null && groupItem.allItems.Count > 0)
                    result.AddRange(groupItem.allItems.Select(t => new IdTitle { id = t.Id.ToString(), title = t.Title }).ToList());

            }

            return result;
        }
    }
}
