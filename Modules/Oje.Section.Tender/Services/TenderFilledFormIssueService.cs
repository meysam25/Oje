using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormIssueService : ITenderFilledFormIssueService
    {
        readonly TenderDBContext db = null;
        readonly ITenderFilledFormPriceService TenderFilledFormPriceService = null;
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly IUserService UserService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public TenderFilledFormIssueService
            (
                TenderDBContext db,
                ITenderFilledFormPriceService TenderFilledFormPriceService,
                IUserService UserService,
                ITenderFilledFormService TenderFilledFormService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService
            )
        {
            this.db = db;
            this.TenderFilledFormPriceService = TenderFilledFormPriceService;
            this.UserService = UserService;
            this.TenderFilledFormService = TenderFilledFormService;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
        }

        public object Create(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            var newItem = new TenderFilledFormIssue()
            {
                CreateDate = DateTime.Now,
                TenderFilledFormId = input.pKey.Value,
                TenderProposalFormJsonConfigId = input.pfId.Value,
                UserId = loginUserId.Value,
                Number = input.insuranceNumber,
                FileUrl = " ",
                Description = input.description,
                IssueDate = input.issueDate.ToEnDate().Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
                newItem.FileUrl = UploadedFileService.UploadNewFile(FileType.IssueTender, input.minPic, TenderFilledFormService.GetUserId(siteSettingId, input.pKey), siteSettingId, newItem.Id, ".jpg,.png,.jpeg,.doc,.docx,.pdf", true);

            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.IssueTender, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, input.pKey), ProposalFilledFormUserType.OwnerUser) }, newItem.Id, "صدور بیمه نامه", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_TenderFilledFormId);
            if (input.pfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Insurance);
            if (string.IsNullOrEmpty(input.issueDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.issueDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.insuranceNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Number);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);

            if (input.id.ToLongReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.id.ToLongReturnZiro() <= 0 && !input.minPic.IsValidExtension(".jpg,.png,.jpeg,.doc,.docx,.pdf"))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (db.TenderFilledFormIssues.Any(t => t.TenderFilledFormId == input.pKey && t.TenderProposalFormJsonConfigId == input.pfId && t.Number == input.insuranceNumber && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Number);

            (int? cityId, int? provinceId) = UserService.GetCityAndProvince(loginUserId);
            if (!TenderFilledFormService.ValidateProvinceAndCity(siteSettingId, input.pKey, provinceId, cityId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!TenderFilledFormService.ValidateOpenCloseDate(siteSettingId, input.pKey))
                throw BException.GenerateNewException(BMessages.Not_Found);
            int? companyId = TenderFilledFormPriceService.GetCompanyIdBy(input.pKey, input.pfId, siteSettingId, loginUserId);
            if (!UserService.HasCompany(loginUserId, companyId))
                throw BException.GenerateNewException(BMessages.No_Company_Exist);
            if (db.TenderFilledFormIssues.Any(t => t.TenderProposalFormJsonConfigId == input.pfId && t.TenderFilledFormId == input.pKey && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public object GetList(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new GlobalGridParentLong();

            var quiryResult = db.TenderFilledFormIssues.Where(t => t.TenderFilledForm.SiteSettingId == siteSettingId && t.TenderFilledFormId == searchInput.pKey && t.UserId == loginUserId);

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.CreateDate,
                    fullName = t.User.Firstname + " " + t.User.Lastname,
                    t.FileUrl,
                    t.Number,
                    insurance = t.TenderProposalFormJsonConfig.ProposalForm.Title,
                    fillId = t.TenderFilledFormId
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.id,
                    issueNumber = t.Number,
                    userfullname = t.fullName,
                    issueDate = t.CreateDate.ToFaDate(),
                    t.insurance,
                    furl = GlobalConfig.FileAccessHandlerUrl + t.FileUrl,
                    t.fillId
                })
                .ToList()
            };
        }

        public object GetListForWeb(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new GlobalGridParentLong();

            var quiryResult = db.TenderFilledFormIssues.Where(t => t.TenderFilledForm.SiteSettingId == siteSettingId && t.TenderFilledFormId == searchInput.pKey && t.TenderFilledForm.UserId == loginUserId);

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.CreateDate,
                    fullName = t.User.Firstname + " " + t.User.Lastname,
                    t.FileUrl,
                    t.Number,
                    insurance = t.TenderProposalFormJsonConfig.ProposalForm.Title,
                    fillId = t.TenderFilledFormId
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.id,
                    issueNumber = t.Number,
                    userfullname = t.fullName,
                    issueDate = t.CreateDate.ToFaDate(),
                    t.insurance,
                    furl = GlobalConfig.FileAccessHandlerUrl + t.FileUrl,
                    t.fillId
                })
                .ToList()
            };
        }

        public object GetBy(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledFormIssues
                .Where(t => t.Id == id && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    pfId = t.TenderProposalFormJsonConfigId,
                    issueDate = t.IssueDate,
                    insuranceNumber = t.Number,
                    minPic_Address = t.FileUrl,
                    description = t.Description
                })
                .ToList()
                .Select(t => new
                {
                    t,
                    id,
                    t.pfId,
                    issueDate = t.issueDate.ToFaDate(),
                    t.insuranceNumber,
                    minPic_Address = GlobalConfig.FileAccessHandlerUrl + t.minPic_Address,
                    t.description
                })
                .FirstOrDefault();
        }

        public object GetByForWeb(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledFormIssues
                .Where(t => t.Id == id && t.TenderFilledForm.SiteSettingId == siteSettingId && t.TenderFilledForm.UserId == loginUserId)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    pfId = t.TenderProposalFormJsonConfigId,
                    issueDate = t.IssueDate,
                    insuranceNumber = t.Number,
                    minPic_Address = t.FileUrl,
                    description = t.Description
                })
                .ToList()
                .Select(t => new
                {
                    t,
                    id,
                    t.pfId,
                    issueDate = t.issueDate.ToFaDate(),
                    t.insuranceNumber,
                    minPic_Address = GlobalConfig.FileAccessHandlerUrl + t.minPic_Address,
                    t.description
                })
                .FirstOrDefault();
        }

        public object Update(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {

            createUpdateValidation(input, siteSettingId, loginUserId);

            var foundItem = db.TenderFilledFormIssues.Where(t => t.Id == input.id && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.TenderProposalFormJsonConfigId = input.pfId.Value;
            foundItem.UserId = loginUserId.Value;
            foundItem.Number = input.insuranceNumber;
            foundItem.Description = input.description;
            foundItem.IssueDate = input.issueDate.ToEnDate().Value;

            if (input.minPic != null && input.minPic.Length > 0)
                foundItem.FileUrl = UploadedFileService.UploadNewFile(FileType.IssueTender, input.minPic, TenderFilledFormService.GetUserId(siteSettingId, input.pKey), siteSettingId, foundItem.Id, ".jpg,.png,.jpeg,.doc,.docx,.pdf", true);

            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.UpdateIssueTender, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, input.pKey), ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "به روز رسانی صدور بیمه نامه", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
