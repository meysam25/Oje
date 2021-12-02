using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;

namespace Oje.Section.InquiryBaseData.Services
{
    public class RoundInqueryManager : IRoundInqueryManager
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        public RoundInqueryManager(InquiryBaseDataDBContext db, AccountManager.Interfaces.ISiteSettingManager SiteSettingManager)
        {
            this.db = db;
            this.SiteSettingManager = SiteSettingManager;
        }

        public ApiResult Create(CreateUpdateRoundInqueryVM input)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            db.Entry(new RoundInquery()
            {
                Format = input.format,
                ProposalFormId = input.formId.Value,
                SiteSettingId = siteSettingId.Value,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdateRoundInqueryVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (string.IsNullOrEmpty(input.format))
                throw BException.GenerateNewException(BMessages.Please_Enter_Format);
            if (input.format != new string('0', input.format.Length))
                throw BException.GenerateNewException(BMessages.Please_Use_0_For_Format);
            if (input.format.Length > 20)
                throw BException.GenerateNewException(BMessages.Format_Can_Not_Be_More_Then_20_Chars);
            if (db.RoundInqueries.Any(t => t.ProposalFormId == input.formId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item)
;
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var deleteItem = db.RoundInqueries.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (deleteItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            db.Entry(deleteItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            return
                db.RoundInqueries
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    format = t.Format,
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    type = t.Type
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.format,
                    t.formId,
                    t.formId_Title,
                    type = (int)t.type
                })
                .FirstOrDefault();
        }

        public GridResultVM<RoundInqueryMainGridResult> GetList(RoundInqueryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new RoundInqueryMainGrid();

            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var qureResult = db.RoundInqueries.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.proposalFormTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.proposalFormTitle));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.format))
                qureResult = qureResult.Where(t => t.Format == searchInput.format);

            int row = searchInput.skip;

            return new GridResultVM<RoundInqueryMainGridResult> 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    format = t.Format,
                    proposalFormTitle = t.ProposalForm.Title,
                    type = t.Type
                })
                .ToList()
                .Select(t => new RoundInqueryMainGridResult 
                {
                    row = ++row,
                    id = t.id,
                    format = t.format,
                    proposalFormTitle = t.proposalFormTitle,
                    type = t.type.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateRoundInqueryVM input)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            var editItem = db.RoundInqueries.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (editItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            editItem.Format = input.format;
            editItem.ProposalFormId = input.formId.Value;
            editItem.Type = input.type.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
