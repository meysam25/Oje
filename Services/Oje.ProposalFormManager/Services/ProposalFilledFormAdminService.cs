﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormAdminService : IProposalFilledFormAdminService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService = null;
        readonly IProposalFilledFormJsonService ProposalFilledFormJsonService = null;
        readonly IProposalFilledFormValueService ProposalFilledFormValueService = null;
        readonly IProposalFilledFormUseService ProposalFilledFormUseService = null;
        readonly IProposalFilledFormCompanyService ProposalFilledFormCompanyService = null;
        readonly IGlobalInqueryService GlobalInqueryService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly Interfaces.IUserService UserService = null;
        readonly IProposalFormPrintDescrptionService ProposalFormPrintDescrptionService = null;
        readonly IAgentRefferService AgentRefferService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IColorService ColorService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFilledFormAdminService(
                ProposalFormDBContext db,
                IProposalFilledFormJsonService ProposalFilledFormJsonService,
                IProposalFilledFormValueService ProposalFilledFormValueService,
                IProposalFilledFormUseService ProposalFilledFormUseService,
                IProposalFilledFormCompanyService ProposalFilledFormCompanyService,
                IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
                IGlobalInqueryService GlobalInqueryService,
                IUploadedFileService UploadedFileService,
                IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService,
                IUserNotifierService UserNotifierService,
                IBankAccountFactorService BankAccountFactorService,
                Interfaces.IUserService UserService,
                IProposalFormPrintDescrptionService ProposalFormPrintDescrptionService,
                IAgentRefferService AgentRefferService,
                IProposalFormService ProposalFormService,
                IColorService ColorService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.ProposalFilledFormAdminBaseQueryService = ProposalFilledFormAdminBaseQueryService;
            this.ProposalFilledFormJsonService = ProposalFilledFormJsonService;
            this.ProposalFilledFormValueService = ProposalFilledFormValueService;
            this.ProposalFilledFormUseService = ProposalFilledFormUseService;
            this.ProposalFilledFormCompanyService = ProposalFilledFormCompanyService;
            this.GlobalInqueryService = GlobalInqueryService;
            this.UploadedFileService = UploadedFileService;
            this.ProposalFilledFormStatusLogService = ProposalFilledFormStatusLogService;
            this.UserNotifierService = UserNotifierService;
            this.BankAccountFactorService = BankAccountFactorService;
            this.UserService = UserService;
            this.ProposalFormPrintDescrptionService = ProposalFormPrintDescrptionService;
            this.AgentRefferService = AgentRefferService;
            this.ProposalFormService = ProposalFormService;
            this.ColorService = ColorService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public object GetUploadImages(GlobalGridParentLong input, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null)
        {
            if (input == null)
                input = new GlobalGridParentLong();
            var foundItemId =
                ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => (status == null && validStatus != null && validStatus.Contains(t.Status)) || t.Status == status)
                .Where(t => t.Id == input.pKey)
                .Select(t => t.Id)
                .FirstOrDefault();

            if (foundItemId <= 0)
                foundItemId = -1;

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, FileType.ProposalFilledForm),
                data = UploadedFileService.GetListBy(foundItemId, FileType.ProposalFilledForm, input.skip, input.take)
            };
        }

        public object Update(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, IFormCollection form)
        {
            var item = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .Select(t => new { id = t.Id, title = t.ProposalForm.Title })
                .FirstOrDefault();
            if (item == null || item.id <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetCacheBy(item.id);
            if (foundJson == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson.JsonConfig);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    ProposalFilledFormValueService.UpdateBy(item.id, form, jsonObj);
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormEdited, ProposalFilledFormUseService.GetProposalFilledFormUserIds(item.id), item.id, item.title, siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + item.id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            foundItem.IsDelete = true;

            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormDeleted, ProposalFilledFormUseService.GetProposalFilledFormUserIds(foundItem.Id), foundItem.Id, "", siteSettingId, "");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var values = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormValues)
                .Include(t => t.ProposalFilledFormKey)
                .AsNoTracking()
                .ToList();
            if (values == null || values.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return values.Select(t => new { key = t.ProposalFilledFormKey.Key, value = t.Value }).ToList();
        }

        public string GetJsonConfir(int id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, string loadUrl, string saveUrl)
        {
            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .Select(t => t.Id)
                .FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetCacheBy(foundId);
            if (foundJson == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson.JsonConfig);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            var allSteps = foundSw.FirstOrDefault().steps;
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);

            foundSw.FirstOrDefault().isEdit = true;
            jsonObj.panels.FirstOrDefault().loadUrl = loadUrl;
            if (foundSw != null && foundSw.FirstOrDefault() != null && foundSw.FirstOrDefault()?.actionOnLastStep != null && foundSw.FirstOrDefault()?.actionOnLastStep.Count == 1)
                foundSw.FirstOrDefault().actionOnLastStep.FirstOrDefault().url = saveUrl;


            return JsonConvert.SerializeObject(jsonObj, EnumService.EnumConverterSetting);
        }

        private List<step> ignoreSteps(List<step> allSteps)
        {
            if (allSteps != null)
                return allSteps.Where(t => t.id != "debitPayment" && t.id != "requiredDocumnet" && t.id != "selectAgent" && t.id != "companyStep").ToList();

            return allSteps;
        }

        public GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status, List<string> roles)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites);

            if (searchInput.cId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProposalFilledFormCompanies.Any(tt => tt.CompanyId == searchInput.cId));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDT.Year && t.CreateDate.Month == targetDT.Month && t.CreateDate.Day == targetDT.Day);
            }
            if (searchInput.price.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.agentFullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.Agent && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.agentFullname)));
            if (!string.IsNullOrEmpty(searchInput.targetUserfullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.OwnerUser && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.targetUserfullname)));
            if (!string.IsNullOrEmpty(searchInput.createUserfullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.CreateUser && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.createUserfullname)));
            if (!string.IsNullOrEmpty(searchInput.targetUserNationalCode))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.OwnerUser && tt.User.Nationalcode == searchInput.targetUserNationalCode));
            if (!string.IsNullOrEmpty(searchInput.fromCreateDate) && searchInput.fromCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.fromCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate >= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.toCreateDate) && searchInput.toCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.toCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate <= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.fromIssueDate) && searchInput.fromIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.fromIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.IssueDate != null && t.IssueDate >= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.toIssueDate) && searchInput.toIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.toIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.IssueDate != null && t.IssueDate <= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.targetUserMobileNumber))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.CreateUser && tt.User.Username.Contains(searchInput.createUserfullname)));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFilledFormMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    cId = t.ProposalFilledFormCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    createDate = t.CreateDate,
                    price = t.Price,
                    agentFullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.Agent).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserMobileNumber = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Username).FirstOrDefault(),
                    createUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.CreateUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserNationalCode = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.CreateUser).Select(tt => tt.User.Nationalcode).FirstOrDefault(),
                    issueDate = t.IssueDate,
                    startDate = t.InsuranceStartDate,
                    endDate = t.InsuranceEndDate,
                    issueFile = t.IssueFile,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFilledFormMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    cId = String.Join(",", t.cId),
                    ppfTitle = t.ppfTitle,
                    createDate = t.createDate.ToFaDate(),
                    price = t.price > 0 ? t.price.ToString("###,###") : "0",
                    agentFullname = t.agentFullname,
                    targetUserfullname = t.targetUserfullname,
                    createUserfullname = t.createUserfullname,
                    targetUserMobileNumber = t.targetUserMobileNumber,
                    issueDate = t.issueDate != null ? t.issueDate.ToFaDate() : "",
                    startDate = t.startDate != null ? t.startDate.ToFaDate() : "",
                    endDate = t.endDate != null ? t.endDate.ToFaDate() : "",
                    isAgent = roles != null && roles.Any(tt => !string.IsNullOrEmpty(tt) && tt.StartsWith("agent")),
                    targetUserNationalCode = t.targetUserNationalCode,
                    issueFile = !string.IsNullOrEmpty(t.issueFile) ? (GlobalConfig.FileAccessHandlerUrl + t.issueFile) : "",
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public object GetRefferUsers(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var result = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == ProposalFilledFormUserType.Refer)
                .Select(t => new
                {
                    id = t.UserId,
                    title = t.User.Firstname + " " + t.User.Lastname + "(" + t.User.Username + ")"
                }).ToList();

            return new
            {
                id = id,
                userIds = result
            };
        }

        public ApiResult CreateUserRefer(CreateUpdateProposalFilledFormUserReffer input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            bool? canSeeOtherWebsites = HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUserReferValidation(input, siteSettingId);
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, canSeeOtherWebsites)
                .Where(t => t.Id == input.id)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == ProposalFilledFormUserType.Refer)
                .AsNoTracking()
                .Select(t => t.UserId)
                .ToList();

            input.userIds = input.userIds.Where(t => !foundItem.Contains(t)).ToList();

            if (input.userIds.Count == 0)
                throw BException.GenerateNewException(BMessages.No_New_User_Added);

            if (canSeeOtherWebsites != true)
                foreach (var tempUserId in input.userIds)
                    if (!db.Users.Any(t => t.Id == tempUserId && t.SiteSettingId == siteSettingId))
                        throw BException.GenerateNewException(BMessages.User_Not_Found);

            foreach (var newUserId in input.userIds)
                ProposalFilledFormUseService.Create(newUserId, ProposalFilledFormUserType.Refer, userId, input.id.ToLongReturnZiro(), (canSeeOtherWebsites == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId));

            UserNotifierService.Notify(userId, UserNotificationType.ReferToUser, ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id.ToLongReturnZiro(), "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id.ToLongReturnZiro());

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUserReferValidation(CreateUpdateProposalFilledFormUserReffer input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.userIds == null || input.userIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
        }

        public object GetCompanies(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormCompanies)
                .Select(t => t.CompanyId)
                .ToList();

            return new { cIds = foundItem, id = id };
        }

        public ApiResult UpdateCompanies(CreateUpdateProposalFilledFormCompany input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            UpdateCompaniesValidation(input, siteSettingId);

            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.GlobalInqueryId > 0)
                throw BException.GenerateNewException(BMessages.You_Can_Not_Edit_Company_Of_ProposalFilledForm_With_Inquiry);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            ProposalFilledFormCompanyService.CreateUpdate(input, userId);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormCompanyChanged, ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void UpdateCompaniesValidation(CreateUpdateProposalFilledFormCompany input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
        }

        public object GetAgent(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var result = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
               .Where(t => t.Id == id)
               .SelectMany(t => t.ProposalFilledFormUsers)
               .Where(t => t.Type == ProposalFilledFormUserType.Agent)
               .Select(t => new
               {
                   id = t.UserId,
                   title = t.User.Firstname + " " + t.User.Lastname + "(" + t.User.Username + ")"
               }).FirstOrDefault();

            return new
            {
                id = id,
                userId = result?.id,
                userId_Title = result?.title
            };
        }

        public object UpdateAgent(long? id, long? userId, int? siteSettingId, long? longUserId, ProposalFilledFormStatus status)
        {
            bool? canSeeOtherWebsites = HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            UpdateAgentValidation(id, userId, siteSettingId);
            var foundUserId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, longUserId, status, canSeeOtherWebsites)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == ProposalFilledFormUserType.Agent)
                .AsNoTracking()
                .Select(t => t.UserId)
                .FirstOrDefault();

            if (foundUserId <= 0)
                ProposalFilledFormUseService.Create(userId, ProposalFilledFormUserType.Agent, longUserId, id.ToLongReturnZiro(), siteSettingId);
            else
                ProposalFilledFormUseService.Update(userId, ProposalFilledFormUserType.Agent, longUserId, id.ToLongReturnZiro(), siteSettingId);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormCompanyChanged, ProposalFilledFormUseService.GetProposalFilledFormUserIds(id.ToLongReturnZiro()), id, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void UpdateAgentValidation(long? id, long? userId, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (!db.Users.Any(t => t.Id == userId && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }

        public ProposalFilledFormPdfVM PdfDetailes(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null)
        {
            var result = new ProposalFilledFormPdfVM();
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
               .Where(t => (status == null && validStatus != null && validStatus.Contains(t.Status)) || t.Status == status)
               .Where(t => t.Id == id)
               .Select(t => new
               {
                   t.Id,
                   t.Price,
                   t.GlobalInqueryId,
                   t.ProposalFormId,
                   t.CreateDate,
                   t.SiteSettingId,
                   agentUserId = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.Agent).Select(tt => tt.UserId).FirstOrDefault(),
                   t.PaymentTraceCode,
                   ppfTitle = t.ProposalForm.Title,
                   createUserFullname = t.ProposalFilledFormUsers.Where(t => t.Type == ProposalFilledFormUserType.OwnerUser).Select(t => t.User.Firstname + " " + t.User.Lastname).FirstOrDefault(),
                   t.IssueFile,
                   selectAgent = t.ProposalFilledFormUsers.Where(t => t.Type == ProposalFilledFormUserType.Agent).Select(t => new
                   {
                       t.User.Firstname,
                       t.User.Lastname,
                       t.User.AgentCode,
                       t.User.Address,
                       t.User.Username,
                       t.User.CompanyTitle
                   })
                   .FirstOrDefault(),
                   values = t.ProposalFilledFormValues.Select(tt => new
                   {
                       tt.Value,
                       Key = tt.ProposalFilledFormKey.Key
                   }).ToList()
               })
               .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetCacheBy(foundItem.Id);
            if (foundJson == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson.JsonConfig);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);
            var fFoundSw = foundSw.FirstOrDefault();
            List<FilledFormPdfGroupVM> listGroup = new();
            List<ProposalFilledFormPaymentVM> foundPaymentList = BankAccountFactorService.GetListBy(BankAccountFactorType.ProposalFilledForm, foundItem.Id, siteSettingId);
            if (foundPaymentList != null && foundPaymentList.Count > 0)
            {
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupPaymentItems = new();
                foreach (var item in foundPaymentList)
                {
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام حساب", value = item.fullName });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "مبلغ", value = item.price.ToString("###,###") + " ریال" });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تاریخ", value = item.payDate.ToFaDate() + " " + item.payDate.ToString("hh:mm") });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد پیگیری", value = item.traceCode });
                }
                listGroup.Add(new FilledFormPdfGroupVM() { title = "وضعیت پرداخت", ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupPaymentItems });
            }
            GlobalInqueryResultVM inquiryInputs = null;
            if (foundItem.GlobalInqueryId > 0)
                inquiryInputs = GlobalInqueryService.GetInquiryDataList(foundItem.GlobalInqueryId.ToLongReturnZiro(), foundItem.ProposalFormId);
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();

                if (inquiryInputs != null && inquiryInputs.inputItems != null && inquiryInputs.inputItems.Count > 0)
                {
                    var thisStepsInputs = inquiryInputs.inputItems.Where(t => t.step == step.id).ToList();
                    if (thisStepsInputs != null && thisStepsInputs.Count > 0)
                        foreach (var input in thisStepsInputs)
                            if (!thisStepsInputs.Any(t => !string.IsNullOrEmpty(t.key) && t.key != input.key && t.key.StartsWith(input.key)) && input.value.IndexOf("tem.Collections.Generic.Lis") == -1)
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = input.title, value = input.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });
                }

                if (allCtrls != null && allCtrls.Count > 0)
                {
                    foreach (var ctrl in allCtrls)
                    {
                        string title = !string.IsNullOrEmpty(ctrl.label) ? ctrl.label : ctrl.ph;
                        string value = foundItem.values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                        if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || ctrl.type == ctrlType.multiRowInput)
                        {
                            if (ctrl.type == ctrlType.checkBox)
                                title = "";
                            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            {
                                for (var i = 0; i < 20; i++)
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = foundItem.values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                            ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                    }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                        else if (ctrl.type == ctrlType.carPlaque)
                        {
                            string value1 = foundItem.values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                            if (!string.IsNullOrEmpty(value1))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value1 });

                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new FilledFormPdfGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }
            Company foundCompany = ProposalFilledFormCompanyService.GetSelectedBy(foundItem.Id);

            if (foundCompany == null)
                foundCompany = ProposalFilledFormCompanyService.GetBy(foundItem.Id);

            result.loginUserWalletBalance = UserService.GetUserWalletBalance(userId, siteSettingId);


            result.proposalFilledFormId = foundItem.Id;
            result.ProposalFilledFormPdfGroupVMs = listGroup;
            result.createUserFullname = foundItem.createUserFullname;
            result.traceCode = foundItem.PaymentTraceCode;
            result.ppfTitle = foundItem.ppfTitle;
            result.agentUserId = foundItem.agentUserId;
            result.id = foundItem.SiteSettingId + "/" + foundItem.ProposalFormId + "/" + foundItem.Id;
            result.price = foundItem.Price;
            result.issueUploadFile = !string.IsNullOrEmpty(foundItem.IssueFile) ? (GlobalConfig.FileAccessHandlerUrl + foundItem.IssueFile) : "";
            result.ppfCreateDate = foundItem.CreateDate.ToFaDate();
            if (inquiryInputs != null && inquiryInputs.inquiryItems != null && inquiryInputs.inquiryItems.Count > 0)
            {
                if (listGroup == null)
                    listGroup = new();
                FilledFormPdfGroupVM newGroupItem = new FilledFormPdfGroupVM() { title = "جزئیات استعلام", ProposalFilledFormPdfGroupItems = new() };
                foreach (var item in inquiryInputs.inquiryItems)
                    newGroupItem.ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = item.title, value = item.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });
                listGroup.Add(newGroupItem);
            }
            if (inquiryInputs != null && inquiryInputs.inputItems != null && inquiryInputs.inputItems.Any(t => string.IsNullOrEmpty(t.step)))
            {
                var exteraParametersList = inquiryInputs.inputItems.Where(t => string.IsNullOrEmpty(t.step)).ToList();
                if (listGroup == null)
                    listGroup = new();
                FilledFormPdfGroupVM newGroupItem = new FilledFormPdfGroupVM() { title = "جزئیات محاسبه استعلام حق بیمه", ProposalFilledFormPdfGroupItems = new() };
                foreach (var item in exteraParametersList)
                {
                    if (!exteraParametersList.Any(t => !string.IsNullOrEmpty(t.key) && t.key != item.key && t.key.StartsWith(item.key)) && item.value.IndexOf("tem.Collections.Generic.Lis") == -1)
                        newGroupItem.ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = item.title, value = item.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });

                }
                listGroup.Add(newGroupItem);
            }
            if (foundCompany != null)
            {
                result.companyTitle = foundCompany.Title;
                result.companyImage = GlobalConfig.FileAccessHandlerUrl + foundCompany.Pic;
            }
            var foundRefferAgent = AgentRefferService.GetBy(foundCompany?.Id, siteSettingId);
            if (foundRefferAgent != null)
            {
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupPaymentItems = new();
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام ", value = foundRefferAgent.FullName });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد ", value = foundRefferAgent.Code });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "همراه ", value = foundRefferAgent.Mobile + "" });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تلفن ", value = foundRefferAgent.Tell + "" });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-12 col-sm-12 col-xs-12 col-lg-12", title = "آدرس ", value = foundRefferAgent.Address + "" });

                listGroup.Add(new FilledFormPdfGroupVM() { title = "واحد معرف", ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupPaymentItems });
            }
            if (foundItem.selectAgent != null)
            {
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupPaymentItems = new();
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام ", value = (!string.IsNullOrEmpty(foundItem.selectAgent.CompanyTitle) ? foundItem.selectAgent.CompanyTitle : (foundItem.selectAgent.Firstname + " " + foundItem.selectAgent.Lastname)) });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد ", value = foundItem.selectAgent.AgentCode + "" });
                if (foundCompany != null)
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "شرکت بیمه ", value = foundCompany.Title + "" });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "همراه ", value = foundItem.selectAgent.Username + "" });
                ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-12 col-sm-12 col-xs-12 col-lg-12", title = "آدرس ", value = foundItem.selectAgent.Address + "" });

                listGroup.Add(new FilledFormPdfGroupVM() { title = "نماینده واحد صدور", ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupPaymentItems });
            }

            result.printDescriptions = ProposalFormPrintDescrptionService.GetList(siteSettingId, foundItem.ProposalFormId);

            return result;
        }

        public ApiResult DeleteUploadImage(long? uploadFileId, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItemId =
               ProposalFilledFormAdminBaseQueryService
               .getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
               .Where(t => t.Id == proposalFilledFormId)
              .Select(t => t.Id)
               .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.Delete(uploadFileId, siteSettingId, proposalFilledFormId, FileType.ProposalFilledForm);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormDocumentDeleted, ProposalFilledFormUseService.GetProposalFilledFormUserIds(proposalFilledFormId.ToLongReturnZiro()), proposalFilledFormId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + proposalFilledFormId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult UploadImage(long? proposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null)
        {
            if (mainFile == null)
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            var foundItemId =
             ProposalFilledFormAdminBaseQueryService
             .getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
             .Where(t => t.Id == proposalFilledFormId)
             .Where(t => (status == null && validStatus != null && validStatus.Contains(t.Status)) || t.Status == status)
             .Select(t => t.Id)
             .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, mainFile, userId, siteSettingId, proposalFilledFormId, ".jpg,.png,.jpeg,.pdf,.doc,.docx", true);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormNewDocument, ProposalFilledFormUseService.GetProposalFilledFormUserIds(proposalFilledFormId.ToLongReturnZiro()), proposalFilledFormId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus((status != null ? status.Value : validStatus.FirstOrDefault())) + "/PdfDetailesForAdmin?id=" + proposalFilledFormId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetStatus(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItemId =
            ProposalFilledFormAdminBaseQueryService
            .getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
            .Where(t => t.Id == id)
           .Select(t => t.Id)
            .FirstOrDefault();
            if (id <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new { status = status, id = id };
        }

        public ApiResult UpdateStatus(ProposalFilledFormChangeStatusVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.status == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Status);
            if (input.status == status)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);

            var foundItem =
            ProposalFilledFormAdminBaseQueryService
            .getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
            .Where(t => t.Id == input.id)
            .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.Status == ProposalFilledFormStatus.NeedSpecialist)
            {
                if (string.IsNullOrEmpty(input.fullname))
                    throw BException.GenerateNewException(BMessages.Please_Enter_Name);
                if (string.IsNullOrEmpty(input.description))
                    throw BException.GenerateNewException(BMessages.Please_Enter_Description);
                if (input.fileList == null || input.fileList.Count == 0)
                    throw BException.GenerateNewException(BMessages.Please_Select_File);
                foreach (var file in input.fileList)
                {
                    if (string.IsNullOrEmpty(file.fileType))
                        throw BException.GenerateNewException(BMessages.Please_Enter_File_Type);
                    if (file.fileType.Length > 100)
                        throw BException.GenerateNewException(BMessages.Validation_Error);
                    if (file.mainFile == null || file.mainFile.Length == 0)
                        throw BException.GenerateNewException(BMessages.Please_Select_File);
                    if (!file.mainFile.IsValidExtension(".jpg,.jpeg,.png,.mp4,.pdf"))
                        throw BException.GenerateNewException(BMessages.Invalid_File);
                }
            }

            if (input.status == ProposalFilledFormStatus.Issuing && (foundItem.InsuranceStartDate == null || foundItem.InsuranceEndDate == null || string.IsNullOrEmpty(foundItem.InsuranceNumber)))
                throw BException.GenerateNewException(BMessages.Change_Status_Can_Not_Be_Done);
            if (input.status == ProposalFilledFormStatus.Issuing && !ProposalFilledFormCompanyService.IsSelectedBy(foundItem.Id))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.status == ProposalFilledFormStatus.Issuing && !ProposalFilledFormUseService.HasAny(foundItem.Id, ProposalFilledFormUserType.Agent))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundItem.Status = input.status.Value;
            foundItem.FilledSignature();
            db.SaveChanges();

            ProposalFilledFormStatusLogService.Create(input.id, input.status, DateTime.Now, userId, input.description, input.fullname, input.fileList, siteSettingId);

            UserNotifierService.Notify(userId, UserNotifierService.ConvertProposalFilledFormStatusToUserNotifiactionType(status), ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetDefaultValuesForIssue(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItem =
           ProposalFilledFormAdminBaseQueryService
           .getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
           .Where(t => t.Id == id)
           .Select(t => new
           {
               id = t.Id,
               ssId = t.SiteSettingId,
               ppId = t.ProposalFormId,
               cid = t.ProposalFilledFormCompanies.Where(tt => tt.IsSelected == true).Select(tt => tt.CompanyId).FirstOrDefault(),
               uid = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.Agent).Select(tt => tt.UserId).FirstOrDefault()
           })
           .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (foundItem.uid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);

            string sDate = DateTime.Now.ToFaDate();
            string eDate = (sDate.Split('/')[0].ToIntReturnZiro() + 1) + "/" + sDate.Split('/')[1] + "/" + sDate.Split('/')[2];

            return new { id = id, startDate = sDate, endDate = eDate };
        }

        public ApiResult IssuePPF(ProposalFilledFormIssueVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            IssuePPFValidation(input, siteSettingId);

            var foundItem =
                   ProposalFilledFormAdminBaseQueryService
                  .getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                  .Where(t => t.Id == input.id)
                  .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            if (input.mainFile != null && input.mainFile.Length > 0)
                foundItem.IssueFile = UploadedFileService.UploadNewFile(FileType.IssueProposalForm, input.mainFile, userId, siteSettingId, foundItem.Id, ".jpg,.jpeg,.png,.doc,.docx,.pdf", true);

            foundItem.InsuranceStartDate = input.startDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceEndDate = input.endDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceNumber = input.insuranceNumber;
            foundItem.Status = ProposalFilledFormStatus.Issuing;

            foundItem.FilledSignature();

            db.SaveChanges();

            ProposalFilledFormStatusLogService.Create(foundItem.Id, ProposalFilledFormStatus.Issuing, DateTime.Now, userId, input.description, null, null, siteSettingId);

            UserNotifierService.Notify(userId, UserNotifierService.ConvertProposalFilledFormStatusToUserNotifiactionType(status), ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id, input.insuranceNumber, siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void IssuePPFValidation(ProposalFilledFormIssueVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.startDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_Start_Date);
            if (input.startDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.endDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_EndDate);
            if (input.endDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.startDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value >= input.endDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.StartDate_Should_Be_Less_Then_EndDate);
            if (string.IsNullOrEmpty(input.insuranceNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Number);
            if (input.insuranceNumber.Length > 50)
                throw BException.GenerateNewException(BMessages.InsuranceNumber_Can_Not_Be_More_Then_50);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (db.ProposalFilledForms.Any(t => t.SiteSettingId == siteSettingId && t.InsuranceNumber == input.insuranceNumber && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (input.mainFile == null || input.mainFile.Length == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(".jpg,.jpeg,.png,.doc,.docx,.pdf"))
                throw BException.GenerateNewException(BMessages.Invalid_File);
        }

        public object GetListForUser(MyProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, List<ProposalFilledFormStatus> validStatus)
        {
            searchInput = searchInput ?? new MyProposalFilledFormMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites).Where(t => validStatus.Contains(t.Status));


            int row = searchInput.skip;

            return new
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    cId = t.ProposalFilledFormCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    createDate = t.CreateDate,
                    price = t.Price,
                    agentFullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.Agent).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserMobileNumber = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Username).FirstOrDefault(),
                    createUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.CreateUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    issueDate = t.IssueDate,
                    startDate = t.InsuranceStartDate,
                    endDate = t.InsuranceEndDate,
                    issueFile = t.IssueFile
                })
                .ToList()
                .Select(t => new ProposalFilledFormMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    cId = String.Join(",", t.cId),
                    ppfTitle = t.ppfTitle,
                    createDate = t.createDate.ToFaDate(),
                    price = t.price > 0 ? t.price.ToString("###,###") : "0",
                    agentFullname = t.agentFullname,
                    targetUserfullname = t.targetUserfullname,
                    createUserfullname = t.createUserfullname,
                    targetUserMobileNumber = t.targetUserMobileNumber,
                    issueDate = t.issueDate != null ? t.issueDate.ToFaDate() : "",
                    startDate = t.startDate != null ? t.startDate.ToFaDate() : "",
                    endDate = t.endDate != null ? t.endDate.ToFaDate() : "",
                    issueFile = !string.IsNullOrEmpty(t.issueFile) ? (GlobalConfig.FileAccessHandlerUrl + t.issueFile) : ""
                })
                .ToList()
            };
        }

        public ProposalFilledFormPdfVM PdfDetailesByForm(IFormCollection form, int? siteSettingId)
        {
            var result = new ProposalFilledFormPdfVM();
            int proposalFormId = form.GetStringIfExist("fid").ToIntReturnZiro();
            long inquiryId = form.GetStringIfExist("inquiryId").ToLongReturnZiro();
            if (proposalFormId <= 0)
                return result;
            var foundProposalForm = ProposalFormService.GetById(proposalFormId, siteSettingId);
            PageForm jsonObj = null;
            try { jsonObj = JsonConvert.DeserializeObject<PageForm>(foundProposalForm.JsonConfig); } catch (Exception) { }
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);
            var fFoundSw = foundSw.FirstOrDefault();
            List<FilledFormPdfGroupVM> listGroup = new();

            GlobalInqueryResultVM inquiryInputs = null;
            if (inquiryId > 0)
                inquiryInputs = GlobalInqueryService.GetInquiryDataList(inquiryId, proposalFormId);

            var values = form.Keys.Select(t => new { Key = t, Value = form.GetStringIfExist(t) }).ToList();
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();

                if (inquiryInputs != null && inquiryInputs.inputItems != null && inquiryInputs.inputItems.Count > 0)
                {
                    var thisStepsInputs = inquiryInputs.inputItems.Where(t => t.step == step.id).ToList();
                    if (thisStepsInputs != null && thisStepsInputs.Count > 0)
                        foreach (var input in thisStepsInputs)
                            if (!thisStepsInputs.Any(t => !string.IsNullOrEmpty(t.key) && t.key != input.key && t.key.StartsWith(input.key)) && input.value.IndexOf("tem.Collections.Generic.Lis") == -1)
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = input.title, value = input.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });
                }

                if (allCtrls != null && allCtrls.Count > 0)
                {
                    foreach (var ctrl in allCtrls)
                    {
                        string title = !string.IsNullOrEmpty(ctrl.label) ? ctrl.label : ctrl.ph;
                        string value = values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                        if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || ctrl.type == ctrlType.multiRowInput)
                        {
                            if (!string.IsNullOrEmpty(ctrl.dataurl) && ctrl.dataurl.Contains("/Core/BaseData/Get/"))
                            {
                                string enumName = ctrl.dataurl.Replace("/Core/BaseData/Get/", "");
                                var allEnums = EnumService.GetEnum(enumName);
                                if (allEnums != null && allEnums.Count > 0)
                                {
                                    var foundItem = allEnums.Where(t => t.id == value).FirstOrDefault();
                                    if (foundItem != null)
                                        value = foundItem.title;
                                }
                            }
                            if (!string.IsNullOrEmpty(ctrl.dataurl) && ctrl.dataurl.ToLower() == "/ProposalFilledForm/Proposal/GetColorList".ToLower())
                            {
                                var foundColor = ColorService.GetById(form.GetStringIfExist(ctrl.name).ToIntReturnZiro());
                                if (foundColor != null)
                                    value = foundColor.Title;
                            }
                            if (ctrl.type == ctrlType.checkBox)
                                title = "";
                            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            {
                                for (var i = 0; i < 20; i++)
                                {
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                        {
                                            ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                        }
                                    }
                                }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new FilledFormPdfGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }

            result.proposalFilledFormId = 0;
            result.ProposalFilledFormPdfGroupVMs = listGroup;
            result.ppfTitle = foundProposalForm.Title;
            result.ppfCreateDate = DateTime.Now.ToFaDate();
            if (inquiryInputs != null && inquiryInputs.inquiryItems != null && inquiryInputs.inquiryItems.Count > 0)
            {
                if (listGroup == null)
                    listGroup = new();
                FilledFormPdfGroupVM newGroupItem = new FilledFormPdfGroupVM() { title = "جزئیات استعلام", ProposalFilledFormPdfGroupItems = new() };
                foreach (var item in inquiryInputs.inquiryItems)
                    newGroupItem.ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = item.title, value = item.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });
                listGroup.Add(newGroupItem);
            }
            if (inquiryInputs != null && inquiryInputs.inputItems != null && inquiryInputs.inputItems.Any(t => string.IsNullOrEmpty(t.step)))
            {
                var exteraParametersList = inquiryInputs.inputItems.Where(t => string.IsNullOrEmpty(t.step)).ToList();
                if (listGroup == null)
                    listGroup = new();
                FilledFormPdfGroupVM newGroupItem = new FilledFormPdfGroupVM() { title = "جزئیات محاسبه استعلام حق بیمه", ProposalFilledFormPdfGroupItems = new() };
                foreach (var item in exteraParametersList)
                {
                    if (!exteraParametersList.Any(t => !string.IsNullOrEmpty(t.key) && t.key != item.key && t.key.StartsWith(item.key)) && item.value.IndexOf("tem.Collections.Generic.Lis") == -1)
                        newGroupItem.ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem { title = item.title, value = item.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });

                }
                listGroup.Add(newGroupItem);
            }

            return result;
        }
    }
}
