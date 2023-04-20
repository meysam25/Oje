using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System.Linq;
using Oje.Section.Tender.Models.DB;
using Oje.Infrastructure.Exceptions;
using Oje.FileService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormPriceService : ITenderFilledFormPriceService
    {
        readonly TenderDBContext db = null;
        readonly IUserService UserService = null;
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TenderFilledFormPriceService
            (
                TenderDBContext db,
                IUserService UserService,
                ITenderFilledFormService TenderFilledFormService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.TenderFilledFormService = TenderFilledFormService;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus tenderSelectStatus)
        {
            createUpdateValidation(input, siteSettingId, loginUserId, tenderSelectStatus);

            var newItem = new TenderFilledFormPrice()
            {
                CompanyId = input.cid.Value,
                CreateDate = DateTime.Now,
                FilledFileUrl = " ",
                UserId = loginUserId.Value,
                TenderFilledFormId = input.pKey.Value,
                Price = input.price.Value,
                IsConfirm = false,
                TenderProposalFormJsonConfigId = input.pfId.Value,
                Description = input.description
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
                newItem.FilledFileUrl =
                    UploadedFileService.UploadNewFile(FileType.TenderPrice, input.minPic, TenderFilledFormService.GetUserId(siteSettingId, input.pKey), siteSettingId, newItem.Id, ".jpg,.png,.jpeg,.doc,.docx,.pdf", true);

            newItem.FilledSignature();
            UserNotifierService.Notify(loginUserId, UserNotificationType.AddTenderPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, input.pKey), ProposalFilledFormUserType.OwnerUser) }, newItem.Id, "تعیین قیمت توسط نماینده", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus tenderSelectStatus)
        {
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (!UserService.HasCompany(loginUserId, input.cid))
                throw BException.GenerateNewException(BMessages.No_Company_Exist);
            if (input.pfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Insurance);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_TenderFilledFormId);
            if (!TenderFilledFormService.ExistByJCId(siteSettingId, input.pKey, input.pfId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!TenderFilledFormService.ValidateCompany(siteSettingId, input.pKey, input.cid))
                throw BException.GenerateNewException(BMessages.Invalid_Company);
            if (!TenderFilledFormService.IsPublished(siteSettingId, input.pKey))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (input.id.ToLongReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.minPic != null && input.minPic.Length > 0 && !input.minPic.IsValidExtension(".jpg,.png,.jpeg,.doc,.docx,.pdf"))
                throw BException.GenerateNewException(BMessages.Invalid_File);
            if (!TenderFilledFormService.ValidateOpenCloseDate(siteSettingId, input.pKey))
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (db.TenderFilledFormPrices.Any(t => t.CompanyId == input.cid && t.TenderProposalFormJsonConfigId == input.pfId && t.UserId == loginUserId && t.TenderFilledFormId == input.pKey && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (db.TenderFilledFormIssues.Any(t => t.TenderFilledFormId == input.pKey))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (
                    !db.TenderFilledForms
                    .selectQuiryFilter(tenderSelectStatus, province, cityid, companyIds, loginUserId)
                    .Where(t => t.Id == input.pKey)
                    .Any()
                )
                throw BException.GenerateNewException(BMessages.Not_Found);

        }

        public GridResultVM<TenderFilledFormPriceMainGridResultVM> GetList(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId, TenderSelectStatus? selectStatus = null)
        {
            searchInput = searchInput ?? new TenderFilledFormPriceMainGrid();

            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);

            var quiryResult = db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, province, cityid, companyIds, loginUserId)
                .SelectMany(t => t.TenderFilledFormPrices)
                .Where(t => t.TenderFilledFormId == searchInput.pKey && t.UserId == loginUserId)
                ;

            if (selectStatus == null)
                quiryResult = quiryResult.getWhereUserIdMultiLevelForUserOwnerShip<TenderFilledFormPrice, User>(loginUserId, canSeeAllItems);

            if (searchInput.code != null)
                quiryResult = quiryResult.Where(t => t.User.AgentCode == searchInput.code);
            if (!string.IsNullOrEmpty(searchInput.insurance))
                quiryResult = quiryResult.Where(t => t.TenderProposalFormJsonConfig.ProposalForm.Title.Contains(searchInput.insurance));
            if (searchInput.company.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.CompanyId == searchInput.company);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsConfirm == searchInput.isActive);
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new GridResultVM<TenderFilledFormPriceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    insurance = t.TenderProposalFormJsonConfig.ProposalForm.Title,
                    createDate = t.CreateDate,
                    company = t.Company.Title,
                    user = t.User.Firstname + " " + t.User.Lastname,
                    isActive = t.IsConfirm,
                    price = t.Price,
                    fid = t.TenderFilledFormId,
                    t.FilledFileUrl,
                    isPub = t.IsPublished,
                    code = t.User.AgentCode
                })
                .ToList()
                .Select(t => new TenderFilledFormPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    company = t.company,
                    createDate = t.createDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    user = t.user,
                    insurance = t.insurance,
                    price = t.price.ToString("###,###"),
                    fid = t.fid,
                    downloadFileUrl = GlobalConfig.FileAccessHandlerUrl + t.FilledFileUrl,
                    isPub = t.isPub == true ? true : false,
                    code = t.code + ""
                })
                .ToList()
            };
        }

        public GridResultVM<TenderFilledFormPriceMainGridResultVM> GetListForWeb(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus)
        {
            searchInput = searchInput ?? new TenderFilledFormPriceMainGrid();


            var quiryResult = db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, null, null, null, loginUserId)
                .SelectMany(t => t.TenderFilledFormPrices)
                .Where(t => t.IsPublished == true && t.TenderFilledFormId == searchInput.pKey)
                ;

            if (!string.IsNullOrEmpty(searchInput.insurance))
                quiryResult = quiryResult.Where(t => t.TenderFilledForm.TenderFilledFormPFs.Any(tt => tt.TenderProposalFormJsonConfig.ProposalForm.Title.Contains(searchInput.insurance)));
            if (searchInput.company.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.CompanyId == searchInput.company);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsConfirm == searchInput.isActive);
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new GridResultVM<TenderFilledFormPriceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    insurance = t.TenderProposalFormJsonConfig.ProposalForm.Title,
                    createDate = t.CreateDate,
                    company = t.Company.Title,
                    user = t.User.Firstname + " " + t.User.Lastname,
                    isActive = t.IsConfirm,
                    price = t.Price,
                    fid = t.TenderFilledFormId,
                    desciption = t.Description,
                    t.FilledFileUrl
                })
                .ToList()
                .Select(t => new TenderFilledFormPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    company = t.company,
                    createDate = t.createDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    user = t.user,
                    insurance = t.insurance,
                    price = t.price.ToString("###,###"),
                    fid = t.fid,
                    desc = !string.IsNullOrEmpty(t.desciption) ? t.desciption : "",
                    downloadFileUrl = GlobalConfig.FileAccessHandlerUrl + t.FilledFileUrl
                })
                .ToList()
            };
        }

        public object GetById(long? id, int? siteSettingId, long? userId, TenderSelectStatus? selectStatus = null)
        {
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(userId);

            return db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, province, cityid, companyIds, userId)
                .SelectMany(t => t.TenderFilledFormPrices)
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => new
                {
                    id = t.Id,
                    cid = t.CompanyId,
                    pfId = t.TenderProposalFormJsonConfigId,
                    price = t.Price,
                    description = t.Description
                })
                .FirstOrDefault();
        }

        public object Update(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus tenderSelectStatus)
        {
            createUpdateValidation(input, siteSettingId, loginUserId, tenderSelectStatus);

            var foundItem = db.TenderFilledFormPrices.Where(t => t.Id == input.id && t.TenderFilledFormId == input.pKey && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundItem.IsConfirm == true || foundItem.IsPublished == true)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundItem.CompanyId = input.cid.Value;
            foundItem.TenderProposalFormJsonConfigId = input.pfId.Value;
            foundItem.Price = input.price.Value;
            foundItem.Description = input.description;
            if (input.minPic != null && input.minPic.Length > 0)
                foundItem.FilledFileUrl =
                    UploadedFileService.UploadNewFile(FileType.TenderPrice, input.minPic, TenderFilledFormService.GetUserId(siteSettingId, input.pKey), siteSettingId, foundItem.Id, ".jpg,.png,.jpeg,.doc,.docx,.pdf", true);

            foundItem.FilledSignature();
            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.UpdateTenderPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, input.pKey), ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "به روز رسانی تعیین قیمت توسط نماینده", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object Delete(long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus tenderSelectStatus)
        {
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);
            if (
                !db.TenderFilledForms
                 .selectQuiryFilter(tenderSelectStatus, province, cityid, companyIds, loginUserId)
                .Any()
                )
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundItem = db.TenderFilledFormPrices.Where(t => t.Id == id && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundItem.IsConfirm == true || foundItem.IsPublished == true)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);
            if (db.TenderFilledFormIssues.Any(t => t.TenderFilledFormId == foundItem.TenderFilledFormId))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.DeleteTenderPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, foundItem.TenderFilledFormId), ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "به روز رسانی تعیین قیمت توسط نماینده", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetUsersForWeb(long? tenderFilledFormId, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db
                .TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, null, null, null, loginUserId)
                .SelectMany(t => t.TenderFilledFormPrices)
                .Where(t => t.TenderFilledFormId == tenderFilledFormId && t.IsPublished == true)
                .GroupBy(t => new { t.Company.Title, t.CompanyId, t.UserId, t.User.Firstname, t.User.Lastname })
                .Select(t => new
                {
                    userId = t.Key.UserId,
                    companyId = t.Key.CompanyId,
                    userFullname = t.Key.Firstname + " " + t.Key.Lastname,
                    companyTitle = t.Key.Title
                })
                .ToList()
                .Select(t => new
                {
                    id = t.userId + "_" + t.companyId,
                    title = t.companyTitle + "(" + t.userFullname + ")"
                })
                .ToList()
                );

            return result;
        }

        public object SelectPrice(MyTenderFilledFormPriceSelectUserUpdateVM input, int? siteSettingId, long? loginUserId, TenderSelectStatus selectStatus)
        {
            selectPriceValidation(input, siteSettingId, loginUserId);

            int? companyId = input.uid.Split('_')[1].ToIntReturnZiro();
            long? userId = input.uid.Split('_')[0].ToLongReturnZiro();

            if (companyId.ToIntReturnZiro() <= 0 || userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var allSelectedPrice =
                db
                .TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, null, null, null, loginUserId)
                .SelectMany(t => t.TenderFilledFormPrices)
                .Where(t => t.TenderFilledFormId == input.pKey && t.IsPublished == true)
                .ToList();

            if (allSelectedPrice == null || allSelectedPrice.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var t in allSelectedPrice)
                if (!t.IsSignature())
                    throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            var curSPrice = allSelectedPrice.Where(t => t.CompanyId == companyId && t.UserId == userId).ToList();
            if (curSPrice == null || curSPrice.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            int countInsuranceExist = TenderFilledFormService.GetInsuranceCount(input.pKey, siteSettingId, loginUserId);
            if (countInsuranceExist != curSPrice.Count)
                throw BException.GenerateNewException(BMessages.User_CanNot_Be_Selected);

            foreach (var price in allSelectedPrice)
            {
                price.IsConfirm = false;
                price.FilledSignature();
            }

            foreach (var price in curSPrice)
            {
                price.IsConfirm = true;
                price.FilledSignature();
            }

            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.TenderSelectPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(userId, ProposalFilledFormUserType.Agent) }, userId, "انتخاب مناقصه گر", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void selectPriceValidation(MyTenderFilledFormPriceSelectUserUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_TenderFilledFormId);
            if (string.IsNullOrEmpty(input.uid))
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (input.uid.Split('_').Length != 2)
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public object Publish(long? id, int? siteSettingId, long? loginUserId, TenderSelectStatus tenderSelectStatus)
        {
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);
            if (
                !db.TenderFilledForms
                 .selectQuiryFilter(tenderSelectStatus, province, cityid, companyIds, loginUserId)
                .Any()
                )
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundPrice = db.TenderFilledFormPrices.Where(t => t.Id == id && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundPrice == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (db.TenderFilledFormIssues.Any(t => t.TenderFilledFormId == foundPrice.TenderFilledFormId))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (!foundPrice.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundPrice.IsPublished = true;
            foundPrice.FilledSignature();
            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.PublishTenderPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(TenderFilledFormService.GetUserId(siteSettingId, foundPrice.TenderFilledFormId), ProposalFilledFormUserType.OwnerUser) }, foundPrice.Id, "به روز رسانی تعیین قیمت توسط نماینده", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetCompanyIdBy(long? tenderFilledFormId, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledFormPrices
                .Where(t => t.TenderFilledFormId == tenderFilledFormId && t.TenderProposalFormJsonConfigId == tenderProposalFormJsonConfigId && t.TenderFilledForm.SiteSettingId == siteSettingId && t.UserId == loginUserId && t.IsConfirm == true && t.IsPublished == true)
                .Select(t => t.CompanyId)
                .FirstOrDefault();
        }
    }
}
