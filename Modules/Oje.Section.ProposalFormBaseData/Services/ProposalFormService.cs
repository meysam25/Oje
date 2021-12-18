using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormService : Interfaces.IProposalFormService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUploadedFileService UploadedFileService = null;

        public ProposalFormService(
                ProposalFormBaseDataDBContext db,
                ISiteSettingService SiteSettingService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(CreateUpdateProposalFormVM input, long? userId)
        {
            CreateValidation(input, userId);

            var newItem = new ProposalForm()
            {
                CreateDate = DateTime.Now,
                CreateUserId = userId.Value,
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                JsonConfig = input.jsonStr,
                Name = input.name,
                ProposalFormCategoryId = input.ppfCatId.Value,
                Title = input.title,
                Type = input.type,
                SiteSettingId = input.siteSettingId
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.rules != null && input.rules.Length > 0)
            {
                newItem.RulesFile = UploadedFileService.UploadNewFile(FileType.ProposalFormRules, input.rules, userId, null, newItem.Id, ".pdf,.doc,.docx", false);
            }
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        void CreateValidation(CreateUpdateProposalFormVM input, long? userId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (userId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First, ApiResultErrorCode.NeedLoginFist);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name, ApiResultErrorCode.ValidationError);
            if (input.ppfCatId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Category, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.jsonStr))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config, ApiResultErrorCode.ValidationError);
            if (db.ProposalForms.Any(t => t.Title == input.title && t.Id != input.id && t.SiteSettingId == input.siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (db.ProposalForms.Any(t => t.Name == input.name && t.Id != input.id && t.SiteSettingId == input.siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Name, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0 && (input.rules == null || input.rules.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm_Rules_File, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ProposalForms.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public GetByIdProposalFormVM GetById(int? id)
        {
            return db.ProposalForms.Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    jsonStr = t.JsonConfig,
                    description = t.Description,
                    isActive = t.IsActive,
                    siteSettingId = t.SiteSettingId,
                    ppfCatId = t.ProposalFormCategoryId,
                    ppfCatId_Title = t.ProposalFormCategory.Title,
                    rules_address = GlobalConfig.FileAccessHandlerUrl + t.RulesFile,
                    type = t.Type
                })
                .ToList()
                .Select(t => new GetByIdProposalFormVM
                {
                    id = t.id,
                    title = t.title,
                    name = t.name,
                    jsonStr = !string.IsNullOrEmpty(t.jsonStr) ? t.jsonStr : "",
                    description = t.description,
                    isActive = t.isActive,
                    siteSettingId = t.siteSettingId,
                    ppfCatId = t.ppfCatId,
                    ppfCatId_Title = t.ppfCatId_Title,
                    rules_address = t.rules_address,
                    type = t.type == null ? "" : ((int)t.type).ToString()
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFormMainGridResultVM> GetList(ProposalFormMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProposalFormMainGrid();

            var qureResult = db.ProposalForms.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.category))
                qureResult = qureResult.Where(t => t.ProposalFormCategory.Title.Contains(searchInput.category));
            if (!string.IsNullOrEmpty(searchInput.name))
                qureResult = qureResult.Where(t => t.Name.Contains(searchInput.name));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.setting != null)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.setting);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));

            var row = searchInput.skip;

            return new GridResultVM<ProposalFormMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    category = t.ProposalFormCategory.Title,
                    isActive = t.IsActive,
                    setting = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    name = t.name,
                    category = t.category,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    setting = t.setting
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProposalFormVM input, long? userId)
        {
            CreateValidation(input, userId);

            var foundItem = db.ProposalForms.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.JsonConfig = input.jsonStr;
            foundItem.Name = input.name;
            foundItem.ProposalFormCategoryId = input.ppfCatId.Value;
            foundItem.Title = input.title;
            foundItem.UpdateUserId = userId.Value;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.Type = input.type;
            foundItem.SiteSettingId = input.siteSettingId;

            if (input.rules != null && input.rules.Length > 0)
            {
                foundItem.RulesFile = UploadedFileService.UploadNewFile(FileType.ProposalFormRules, input.rules, userId, null, foundItem.Id, ".pdf,.doc,.docx", false);
            }

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public bool Exist(int id, int? siteSettingId)
        {
            return db.ProposalForms.Any(t => t.Id == id && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null));
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

            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var qureResult = db.ProposalForms.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId || t.SiteSettingId == null);
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
