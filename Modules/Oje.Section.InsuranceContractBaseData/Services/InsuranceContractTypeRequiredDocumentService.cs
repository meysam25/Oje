using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractTypeRequiredDocumentService : IInsuranceContractTypeRequiredDocumentService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public InsuranceContractTypeRequiredDocumentService
            (
                InsuranceContractBaseDataDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(InsuranceContractTypeRequiredDocumentCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new InsuranceContractTypeRequiredDocument()
            {
                CreateDate = DateTime.Now,
                InsuranceContractId = input.cid.Value,
                InsuranceContractTypeId = input.ctId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsRequired = input.isRequired.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.downloadFile != null && input.downloadFile.Length > 0)
            {
                newItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.ContractUploadFileSample, input.downloadFile, null, siteSettingId, newItem.Id, ".doc,.docx,.jpg,.jpeg,.png,.pdf", false);
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(InsuranceContractTypeRequiredDocumentCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (input.ctId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (db.InsuranceContractTypeRequiredDocuments.Any(t => t.Id != input.id && t.Title == input.title && t.SiteSettingId == siteSettingId && t.InsuranceContractId == input.cid && t.InsuranceContractTypeId == input.ctId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.InsuranceContractTypeRequiredDocuments.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public InsuranceContractTypeRequiredDocumentCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.InsuranceContractTypeRequiredDocuments
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    t.Id,
                    t.InsuranceContractId,
                    t.InsuranceContractTypeId,
                    t.Title,
                    t.IsActive,
                    t.IsRequired,
                    t.DownloadFile
                })
                .Take(1)
                .Select(t => new InsuranceContractTypeRequiredDocumentCreateUpdateVM
                {
                    cid = t.InsuranceContractId,
                    ctId = t.InsuranceContractTypeId,
                    downloadFile_address = !string.IsNullOrEmpty(t.DownloadFile) ? (GlobalConfig.FileAccessHandlerUrl + t.DownloadFile) : "",
                    id = t.Id,
                    isActive = t.IsActive,
                    isRequired = t.IsRequired,
                    title = t.Title
                }).FirstOrDefault();
        }

        public GridResultVM<InsuranceContractTypeRequiredDocumentMainGridResultVM> GetList(InsuranceContractTypeRequiredDocumentMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new InsuranceContractTypeRequiredDocumentMainGrid();

            var qureResult = db.InsuranceContractTypeRequiredDocuments.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.cid.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractId == searchInput.cid);
            if (searchInput.ctId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractTypeId == searchInput.ctId);
            if (searchInput.isRequired != null)
                qureResult = qureResult.Where(t => t.IsRequired == searchInput.isRequired);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractTypeRequiredDocumentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    cTitle = t.InsuranceContract.Title,
                    ctTitle = t.InsuranceContractType.Title,
                    t.IsActive,
                    t.IsRequired
                })
                .ToList()
                .Select(t => new InsuranceContractTypeRequiredDocumentMainGridResultVM
                {
                    row = ++row,
                    id = t.Id,
                    cid = t.cTitle,
                    ctId = t.ctTitle,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    isRequired = t.IsRequired == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    title = t.Title
                })
                .ToList()
            };
        }

        public ApiResult Update(InsuranceContractTypeRequiredDocumentCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.InsuranceContractTypeRequiredDocuments.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.UpdateDate = DateTime.Now;
            foundItem.InsuranceContractId = input.cid.Value;
            foundItem.InsuranceContractTypeId = input.ctId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsRequired = input.isRequired.ToBooleanReturnFalse();
            foundItem.Title = input.title;

            if (input.downloadFile != null && input.downloadFile.Length > 0)
                foundItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.ContractUploadFileSample, input.downloadFile, null, siteSettingId, foundItem.Id, ".doc,.docx,.jpg,.jpeg,.png,.pdf", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public RequiredDocumentVM GetRequiredDocuments(int? insuranceContractId, int? insuranceContractTypeId, int? siteSettingId, string typeDescrtiption)
        {
            return new RequiredDocumentVM
            {
                desc = typeDescrtiption,
                items = db.InsuranceContractTypeRequiredDocuments
                .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.InsuranceContractTypeId == insuranceContractTypeId && t.InsuranceContractId == insuranceContractId)
                .Select(t => new
                {
                    title = t.Title,
                    isRequired = t.IsRequired,
                    sample = t.DownloadFile
                })
                .ToList()
                .Select(t => new 
                {
                    t.title,
                    t.isRequired,
                    sample = !string.IsNullOrEmpty(t.sample) ? (GlobalConfig.FileAccessHandlerUrl + t.sample) : ""
                })
                .ToList()
            };
        }
    }
}
