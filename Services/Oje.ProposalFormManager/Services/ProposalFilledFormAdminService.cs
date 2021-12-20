using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        readonly AccountService.Interfaces.IUploadedFileService UploadedFileService = null;
        readonly IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public ProposalFilledFormAdminService(
                ProposalFormDBContext db,
                AccountService.Interfaces.IUserService UserService,
                IProposalFilledFormJsonService ProposalFilledFormJsonService,
                IProposalFilledFormValueService ProposalFilledFormValueService,
                IProposalFilledFormUseService ProposalFilledFormUseService,
                IProposalFilledFormCompanyService ProposalFilledFormCompanyService,
                IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
                IGlobalInqueryService GlobalInqueryService,
                AccountService.Interfaces.IUploadedFileService UploadedFileService,
                IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService,
                IUserNotifierService UserNotifierService
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
        }

        public object GetUploadImages(GlobalGridParentLong input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (input == null)
                input = new GlobalGridParentLong();
            var foundItemId =
                ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == input.pKey)
               .Select(t => t.Id)
                .FirstOrDefault();

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, FileType.ProposalFilledForm),
                data = UploadedFileService.GetListBy(foundItemId, FileType.ProposalFilledForm, input.skip, input.take)
            };
        }

        public object Update(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status, IFormCollection form)
        {
            var item = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == id)
                .Select(t => new { id = t.Id, title = t.ProposalForm.Title })
                .FirstOrDefault();
            if (item == null || item.id <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetBy(item.id);
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
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsDelete = true;

            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormDeleted, ProposalFilledFormUseService.GetProposalFilledFormUserIds(foundItem.Id), foundItem.Id, "", siteSettingId, "");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var values = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
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
            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == id)
                .Select(t => t.Id)
                .FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetBy(foundId);
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

        public GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status);

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
                    issueDate = t.IssueDate,
                    startDate = t.InsuranceStartDate,
                    endDate = t.InsuranceEndDate
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
                    endDate = t.endDate != null ? t.endDate.ToFaDate() : ""
                })
                .ToList()
            };
        }

        public object GetRefferUsers(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var result = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
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
            CreateUserReferValidation(input, siteSettingId);
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == input.id)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == ProposalFilledFormUserType.Refer)
                .AsNoTracking()
                .Select(t => t.UserId)
                .ToList();

            input.userIds = input.userIds.Where(t => !foundItem.Contains(t)).ToList();

            if (input.userIds.Count == 0)
                throw BException.GenerateNewException(BMessages.No_New_User_Added);

            foreach (var newUserId in input.userIds)
                ProposalFilledFormUseService.Create(newUserId, ProposalFilledFormUserType.Refer, userId, input.id.ToLongReturnZiro());

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
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormCompanies)
                .Select(t => t.CompanyId)
                .ToList();

            return new { cIds = foundItem, id = id };
        }

        public ApiResult UpdateCompanies(CreateUpdateProposalFilledFormCompany input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            UpdateCompaniesValidation(input, siteSettingId);

            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.GlobalInqueryId > 0)
                throw BException.GenerateNewException(BMessages.You_Can_Not_Edit_Company_Of_ProposalFilledForm_With_Inquiry);

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
            var result = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
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
            UpdateAgentValidation(id, userId, siteSettingId);
            var foundUserId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, longUserId, status)
                .Where(t => t.Id == id)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == ProposalFilledFormUserType.Agent)
                .AsNoTracking()
                .Select(t => t.UserId)
                .FirstOrDefault();

            if (foundUserId <= 0)
                ProposalFilledFormUseService.Create(userId, ProposalFilledFormUserType.Agent, longUserId, id.ToLongReturnZiro());
            else
                ProposalFilledFormUseService.Update(userId, ProposalFilledFormUserType.Agent, longUserId, id.ToLongReturnZiro());

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
        }

        public ProposalFilledFormPdfVM PdfDetailes(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var result = new ProposalFilledFormPdfVM();
            var foundItem = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status)
               .Where(t => t.Id == id)
               .Select(t => new
               {
                   t.Id,
                   t.GlobalInqueryId,
                   t.ProposalFormId,
                   t.CreateDate,
                   t.SiteSettingId,
                   ppfTitle = t.ProposalForm.Title,
                   createUserFullname = t.ProposalFilledFormUsers.Where(t => t.Type == ProposalFilledFormUserType.OwnerUser).Select(t => t.User.Firstname + " " + t.User.Lastname).FirstOrDefault(),
                   values = t.ProposalFilledFormValues.Select(tt => new
                   {
                       tt.Value,
                       Key = tt.ProposalFilledFormKey.Key
                   }).ToList()
               })
               .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = ProposalFilledFormJsonService.GetBy(foundItem.Id);
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
            List<ProposalFilledFormPdfGroupVM> listGroup = new();
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<ProposalFilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();
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
                                {
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = foundItem.values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                        {
                                            ProposalFilledFormPdfGroupItems.Add(new ProposalFilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                        }
                                    }
                                }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new ProposalFilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new ProposalFilledFormPdfGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }


            result.ProposalFilledFormPdfGroupVMs = listGroup;
            result.createUserFullname = foundItem.createUserFullname;
            result.ppfTitle = foundItem.ppfTitle;
            result.id = foundItem.SiteSettingId + "/" + foundItem.ProposalFormId + "/" + foundItem.Id;
            result.ppfCreateDate = foundItem.CreateDate.ToFaDate();
            if (foundItem.GlobalInqueryId > 0)
                GlobalInqueryService.AppendInquiryData(foundItem.GlobalInqueryId.ToLongReturnZiro(), result.ProposalFilledFormPdfGroupVMs);
            Company foundCompany = ProposalFilledFormCompanyService.GetSelectedBy(foundItem.Id);
            if (foundCompany != null)
            {
                result.companyTitle = foundCompany.Title;
                result.companyImage = GlobalConfig.FileAccessHandlerUrl + foundCompany.Pic;
            }

            return result;
        }

        public ApiResult DeleteUploadImage(long? uploadFileId, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItemId =
               ProposalFilledFormAdminBaseQueryService
               .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
               .Where(t => t.Id == proposalFilledFormId)
              .Select(t => t.Id)
               .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.Delete(uploadFileId, siteSettingId, proposalFilledFormId, FileType.ProposalFilledForm);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormDocumentDeleted, ProposalFilledFormUseService.GetProposalFilledFormUserIds(proposalFilledFormId.ToLongReturnZiro()), proposalFilledFormId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + proposalFilledFormId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult UploadImage(long? proposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (mainFile == null)
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            var foundItemId =
             ProposalFilledFormAdminBaseQueryService
             .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
             .Where(t => t.Id == proposalFilledFormId)
            .Select(t => t.Id)
             .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, mainFile, userId, siteSettingId, proposalFilledFormId, ".jpg,.png,.jpeg,.pdf,.doc,.docx", true);

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormNewDocument, ProposalFilledFormUseService.GetProposalFilledFormUserIds(proposalFilledFormId.ToLongReturnZiro()), proposalFilledFormId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + proposalFilledFormId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetStatus(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItemId =
            ProposalFilledFormAdminBaseQueryService
            .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
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

            var foundItem =
            ProposalFilledFormAdminBaseQueryService
            .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
            .Where(t => t.Id == input.id)
            .FirstOrDefault();

            if (input.status == ProposalFilledFormStatus.Issuing && (foundItem.InsuranceStartDate == null || foundItem.InsuranceEndDate == null || string.IsNullOrEmpty(foundItem.InsuranceNumber)))
                throw BException.GenerateNewException(BMessages.Change_Status_Can_Not_Be_Done);
            if (input.status == ProposalFilledFormStatus.Issuing && !ProposalFilledFormCompanyService.IsSelectedBy(foundItem.Id))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.status == ProposalFilledFormStatus.Issuing && !ProposalFilledFormUseService.HasAny(foundItem.Id, ProposalFilledFormUserType.Agent))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Status = input.status.Value;
            db.SaveChanges();

            ProposalFilledFormStatusLogService.Create(input.id, input.status, DateTime.Now, userId, input.description);

            UserNotifierService.Notify(userId, UserNotifierService.ConvertProposalFilledFormStatusToUserNotifiactionType(status), ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetDefaultValuesForIssue(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundItem =
           ProposalFilledFormAdminBaseQueryService
           .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
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

            return new { id = id, startDate = sDate, endDate = eDate, insuranceNumber = DateTime.Now.ToFaDate() + "/" + foundItem.ssId + "/" + foundItem.ppId + "/" + foundItem.cid + "/" + foundItem.uid + "/" + foundItem.id };
        }

        public ApiResult IssuePPF(ProposalFilledFormIssueVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            IssuePPFValidation(input, siteSettingId);

            var foundItem =
                   ProposalFilledFormAdminBaseQueryService
                  .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                  .Where(t => t.Id == input.id)
                  .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.InsuranceStartDate = input.startDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceEndDate = input.endDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceNumber = input.insuranceNumber;
            foundItem.Status = ProposalFilledFormStatus.Issuing;
            db.SaveChanges();

            ProposalFilledFormStatusLogService.Create(foundItem.Id, ProposalFilledFormStatus.Issuing, DateTime.Now, userId, input.description);

            UserNotifierService.Notify(userId, UserNotifierService.ConvertProposalFilledFormStatusToUserNotifiactionType(status), ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.id.ToLongReturnZiro()), input.id, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.id);

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
                throw BException.GenerateNewException(BMessages.Please_Enter_InsuranceNumber);
            if (input.insuranceNumber.Length > 50)
                throw BException.GenerateNewException(BMessages.InsuranceNumber_Can_Not_Be_More_Then_50);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (db.ProposalFilledForms.Any(t => t.SiteSettingId == siteSettingId && t.InsuranceNumber == input.insuranceNumber && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }
    }
}
