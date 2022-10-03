using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFormReminderService : IProposalFormReminderService
    {
        readonly ProposalFormDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFormReminderService
            (
                ProposalFormDBContext db,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ReminderCreateVM input, int? siteSettingId, IpSections ipSections, long? loginUserId)
        {
            CreateValidation(input, siteSettingId, ipSections);

            var newItem = new ProposalFormReminder()
            {
                CreateDate = DateTime.Now,
                Description = input.summery,
                Ip1 = ipSections.Ip1,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                Mobile = input.mobile.ToLongReturnZiro(),
                ProposalFormId = input.fid.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.Value,
                TargetDate = input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                LoginUserId = loginUserId
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.nationalCard != null && input.nationalCard.Length > 0)
                newItem.NationalCardImage = UploadedFileService.UploadNewFile(FileType.ProposalFilledFormReminder, input.nationalCard, loginUserId, siteSettingId, null, ".png,.jpg,.jpeg", true);

            if (input.insuranceImage != null && input.insuranceImage.Length > 0)
                newItem.PrevInsuranceImage = UploadedFileService.UploadNewFile(FileType.ProposalFilledFormReminder, input.insuranceImage, loginUserId, siteSettingId, null, ".png,.jpg,.jpeg", true);

            db.SaveChanges();

            UserNotifierService.Notify
                       (
                           loginUserId,
                           UserNotificationType.ProposalFormReminder,
                           new System.Collections.Generic.List<PPFUserTypes>(),
                           null,
                           "ثبت یادآوری جدید",
                           siteSettingId, "/ProposalFilledForm/ProposalFormReminder/Index"
                       );

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.ProposalFormReminders
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.ProposalFormReminders
                .OrderByDescending(t => t.CreateDate)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.CreateDate,
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.ProposalFormId,
                    ProposalFormTitle = t.ProposalForm.Title,
                    catId = t.ProposalForm.ProposalFormCategoryId,
                    catId_title = t.ProposalForm.ProposalFormCategory.Title,
                    t.TargetDate,
                    t.Mobile,
                    t.Description,
                    t.NationalCardImage,
                    t.PrevInsuranceImage,
                    t.Id
                })
                .ToList()
                .Select(t => new
                {
                    id = t.Id,
                    appfCatId = t.catId,
                    appfCatId_Title = t.catId_title,
                    fid = t.ProposalFormId,
                    fid_Title = t.ProposalFormTitle,
                    targetDate = t.TargetDate.ToFaDate(),
                    mobile = "0" + t.Mobile,
                    summery = t.Description,
                    insuranceImage_address = !string.IsNullOrEmpty(t.PrevInsuranceImage) ? (GlobalConfig.FileAccessHandlerUrl + t.PrevInsuranceImage) : "",
                    nationalCard_address = !string.IsNullOrEmpty(t.NationalCardImage) ? (GlobalConfig.FileAccessHandlerUrl + t.NationalCardImage) : ""
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFormReminderMainGridResultVM> GetList(ProposalFormReminderMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new ProposalFormReminderMainGrid();

            var curDate = DateTime.Now;
            var feacherDT = curDate.AddYears(300);

            var qureResult = db.ProposalFormReminders.OrderBy(t => t.TargetDate >= curDate ? t.TargetDate : feacherDT)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (searchInput.mobile.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Mobile == searchInput.mobile);
            if (!string.IsNullOrEmpty(searchInput.td) && searchInput.td.ToEnDate() != null)
            {
                var tdOb = searchInput.td.ToEnDate().Value;
                qureResult = qureResult.Where(t => t.TargetDate.Year == tdOb.Year && t.TargetDate.Month == tdOb.Month && t.TargetDate.Day == tdOb.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormReminderMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    t.CreateDate,
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    ppfTitle = t.ProposalForm.Title,
                    mobile = t.Mobile,
                    td = t.TargetDate,
                    t.Id,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormReminderMainGridResultVM
                {
                    row = ++row,
                    id = t.Id,
                    mobile = "0" + t.mobile,
                    ppfTitle = t.ppfTitle,
                    td = t.td.ToFaDate(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()

            };
        }

        public ApiResult Update(ReminderCreateVM input, int? siteSettingId, long? userId)
        {
            CreateValidation(input, siteSettingId, null, true);

            var foundItem = db.ProposalFormReminders
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.summery;
            foundItem.Mobile = input.mobile.ToLongReturnZiro();
            foundItem.ProposalFormId = input.fid.ToIntReturnZiro();
            foundItem.TargetDate = input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.LoginUserId = userId;

            if (input.nationalCard != null && input.nationalCard.Length > 0)
                foundItem.NationalCardImage = UploadedFileService.UploadNewFile(FileType.ProposalFilledFormReminder, input.nationalCard, userId, siteSettingId, null, ".png,.jpg,.jpeg", true);

            if (input.insuranceImage != null && input.insuranceImage.Length > 0)
                foundItem.PrevInsuranceImage = UploadedFileService.UploadNewFile(FileType.ProposalFilledFormReminder, input.insuranceImage, userId, siteSettingId, null, ".png,.jpg,.jpeg", true);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(ReminderCreateVM input, int? siteSettingId, IpSections ipSections, bool ignoreIpSection = false)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (ignoreIpSection == false && ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.targetDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (!string.IsNullOrEmpty(input.summery) && input.summery.Length > 4000)
                throw BException.GenerateNewException(BMessages.To_Much_Summery);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate() <= DateTime.Now)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);
        }
    }
}
