using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormService : IGeneralFormService
    {
        readonly GeneralFormDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        static string acceptFileExtension = ".pdf,.doc,.docx";
        public GeneralFormService
            (
                GeneralFormDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(GeneralFormCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var newItem = new GeneralForm()
            {
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                JsonConfig = input.jsonStr,
                Name = input.name,
                Title = input.title,
                SiteSettingId = input.siteSettingId,
                TermTemplate = input.termT,
                RulesFile = " ",
                ContractFile = " "
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.rules != null && input.rules.Length > 0)
                newItem.RulesFile = UploadedFileService.UploadNewFile(FileType.ProposalFormRules, input.rules, null, null, newItem.Id, acceptFileExtension, false);
            if (input.conteractFile != null && input.conteractFile.Length > 0)
                newItem.ContractFile = UploadedFileService.UploadNewFile(FileType.ProposalFormContractRules, input.conteractFile, null, null, newItem.Id, acceptFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(GeneralFormCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name, ApiResultErrorCode.ValidationError);
            if (input.name.Length > 100)
                throw BException.GenerateNewException(BMessages.Name_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.jsonStr))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0 && (input.rules == null || input.rules.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm_Rules_File, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (string.IsNullOrEmpty(input.termT))
                throw BException.GenerateNewException(BMessages.Please_Enter_Terms);
            if (input.termT.Length > 4000)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.rules != null && input.rules.Length > 0 && !input.rules.IsValidExtension(acceptFileExtension))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (input.conteractFile != null && input.conteractFile.Length > 0 && !input.conteractFile.IsValidExtension(acceptFileExtension))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
        }

        public ApiResult Delete(long? id)
        {
            var foundItem = db.GeneralForms.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id)
        {
            return db.GeneralForms
                .Where(t => t.Id == id)
                .Select(t => new GeneralFormCreateUpdateVM
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    jsonStr = t.JsonConfig,
                    description = t.Description,
                    isActive = t.IsActive,
                    siteSettingId = t.SiteSettingId,
                    rules_address = !string.IsNullOrEmpty(t.RulesFile) ? GlobalConfig.FileAccessHandlerUrl + t.RulesFile : "",
                    conteractFile_address = !string.IsNullOrEmpty(t.ContractFile) ? GlobalConfig.FileAccessHandlerUrl + t.ContractFile : "",
                    termT = t.TermTemplate
                })
                .FirstOrDefault();
        }

        public GridResultVM<GeneralFormMainGridResultVM> GetList(GeneralFormMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new GeneralFormMainGrid();

            var qureResult = db.GeneralForms.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.name))
                qureResult = qureResult.Where(t => t.Name.Contains(searchInput.name));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.setting != null)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.setting);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));

            var row = searchInput.skip;

            return new GridResultVM<GeneralFormMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive,
                    setting = t.SiteSetting.Title,
                })
                .ToList()
                .Select(t => new GeneralFormMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    name = t.name,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    setting = t.setting
                })
                .ToList()
            };
        }

        public ApiResult Update(GeneralFormCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.GeneralForms.Where(t => t.Id == input.id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.JsonConfig = input.jsonStr;
            foundItem.Name = input.name;
            foundItem.Title = input.title;
            foundItem.SiteSettingId = input.siteSettingId;
            foundItem.TermTemplate = input.termT;

            if (input.rules != null && input.rules.Length > 0)
                foundItem.RulesFile = UploadedFileService.UploadNewFile(FileType.ProposalFormRules, input.rules, null, null, foundItem.Id, acceptFileExtension, false);
            if (input.conteractFile != null && input.conteractFile.Length > 0)
                foundItem.ContractFile = UploadedFileService.UploadNewFile(FileType.ProposalFormContractRules, input.conteractFile, null, null, foundItem.Id, acceptFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId = null)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.GeneralForms.OrderByDescending(t => t.Id).AsQueryable();
            if (siteSettingId != null)
                qureResult = qureResult.Where(t => t.SiteSettingId == null || t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public string GetJSonConfigFile(int id, int? siteSettingId)
        {
            return db.GeneralForms.Where(t => t.Id == id && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).Select(t => t.JsonConfig).FirstOrDefault();
        }

        public bool Exist(long id, int? siteSettingId)
        {
            return db.GeneralForms.Any(t => t.Id == id && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null));
        }

        public GeneralForm GetByIdNoTracking(long id, int? siteSettingId)
        {
            return db.GeneralForms.Where(t => t.Id == id && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId)).AsNoTracking().FirstOrDefault();
        }
    }
}
