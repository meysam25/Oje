using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderProposalFormJsonConfigFileService : ITenderProposalFormJsonConfigFileService
    {
        readonly TenderDBContext db = null;
        public TenderProposalFormJsonConfigFileService
            (
                TenderDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(TenderProposalFormJsonConfigFileCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new TenderProposalFormJsonConfigFile()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsRequired = input.isRequired.ToBooleanReturnFalse(),
                Name = input.name,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                TenderProposalFormJsonConfigId = input.fid.ToIntReturnZiro(),
                Title = input.title,
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderProposalFormJsonConfigFileCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (input.name.Length > 100)
                throw BException.GenerateNewException(BMessages.Name_Can_Not_Be_More_Then_100_chars);
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!db.TenderProposalFormJsonConfigs.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.fid))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (db.TenderProposalFormJsonConfigFiles.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Name == input.name && t.TenderProposalFormJsonConfigId == input.fid))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TenderProposalFormJsonConfigFiles.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.TenderProposalFormJsonConfigFiles
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive,
                    isRequired = t.IsRequired,
                    fid = t.TenderProposalFormJsonConfigId
                })
                .FirstOrDefault();
        }

        public GridResultVM<TenderProposalFormJsonConfigFileMainGridResultVM> GetList(TenderProposalFormJsonConfigFileMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.TenderProposalFormJsonConfigFiles.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.fid))
                quiryResult = quiryResult.Where(t => t.TenderProposalFormJsonConfig.ProposalForm.Title.Contains(searchInput.fid));
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<TenderProposalFormJsonConfigFileMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    fid = t.TenderProposalFormJsonConfig.ProposalForm.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new TenderProposalFormJsonConfigFileMainGridResultVM
                {
                    row = ++row,
                    fid = t.fid,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(TenderProposalFormJsonConfigFileCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.TenderProposalFormJsonConfigFiles.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Name = input.name;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsRequired = input.isRequired.ToBooleanReturnFalse();
            foundItem.TenderProposalFormJsonConfigId = input.fid.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public string GetJsonConfigBy(int? ppfid, int? siteSettingId)
        {
            string result = "{}";

            var files = db.TenderProposalFormJsonConfigFiles
                .Where(t => t.SiteSettingId == siteSettingId && t.TenderProposalFormJsonConfigId == ppfid && t.IsActive == true)
                .Select(t => new 
                {
                    t.Id,
                    t.Name,
                    t.Title,
                    t.IsRequired,
                    FTItle = t.TenderProposalFormJsonConfig.ProposalForm.Title
                })
                .ToList();

            if (files != null && files.Count > 0)
            {
                result = "{ \"panels\": [{ \"title\": \""+ files.FirstOrDefault()?.FTItle +"\",";

                result += @"
                ""stepWizards"": [
                {
		          ""moveBackButtonToTop"": true,
                  ""steps"": [
			        {
                      ""order"": 1,
                      ""title"": ""مدارک"",
			          ""hideMoveNextButton"": true,
                      ""panels"": [
                        {
                          ""ctrls"": [";

                foreach (var file in files)
                {
                    result += "{ \"parentCL\": \"col-xl-4 col-lg-4 col-md-3 col-sm-6 col-xs-12\", \"name\": \"" + file.Name + "_" + file.Id + "\", \"type\": \"file\", \"label\": \""+ file.Title + "\", \"acceptEx\": \".jpg,.png,.jpeg,.pdf,.zip\", \"hideImagePreview\": true, \"isRequired\": "+ file.IsRequired.ToString().ToLower() +" }";
                    result += ",";
                }
                result += "{ \"parentCL\": \"col-md-12 col-sm-12 col-xs-12 col-lg-12\", \"type\": \"empty\" }, { \"parentCL\": \"col-md-8 col-sm-6 col-xs-12 col-lg-8\", \"type\": \"empty\" }, { \"parentCL\": \"col-md-4 col-sm-6 col-xs-12 col-lg-4\", \"class\": \"btn-primary btn-block \", \"type\": \"button\", \"title\": \"تایید و ادامه\", \"onClick\": \"addNewRowForMultiCtrl(this)\" }";

                result += @"]
                        }
                      ]
                    }
                  ]
                }
              ] ";

                result += "}]}";
            }
                

            return result;
        }

        public List<TenderProposalFormJsonConfigFile> GetFilesBy(int? siteSettingId, int fid)
        {
            return db.TenderProposalFormJsonConfigFiles.Where(t => t.SiteSettingId == siteSettingId && t.TenderProposalFormJsonConfigId == fid && t.IsActive == true).AsNoTracking().ToList();
        }
    }
}
