using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
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
    public class InsuranceContractProposalFilledFormService : IInsuranceContractProposalFilledFormService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IInsuranceContractProposalFilledFormJsonService InsuranceContractProposalFilledFormJsonService = null;
        readonly IInsuranceContractProposalFilledFormUserService InsuranceContractProposalFilledFormUserService = null;
        readonly IInsuranceContractProposalFilledFormValueService InsuranceContractProposalFilledFormValueService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly Sms.Interfaces.ISmsValidationHistoryService SmsValidationHistoryService = null;
        readonly IUserService UserService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        const int year18DaysCount = 365 * 18; //6570
        const int maxInputUsers = 21;

        public InsuranceContractProposalFilledFormService
            (
                InsuranceContractBaseDataDBContext db,
                IInsuranceContractService InsuranceContractService,
                IInsuranceContractProposalFilledFormJsonService InsuranceContractProposalFilledFormJsonService,
                IInsuranceContractProposalFilledFormUserService InsuranceContractProposalFilledFormUserService,
                IInsuranceContractProposalFilledFormValueService InsuranceContractProposalFilledFormValueService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService,
                IUserService UserService,
                IHttpContextAccessor HttpContextAccessor,
                Sms.Interfaces.ISmsValidationHistoryService SmsValidationHistoryService
            )
        {
            this.db = db;
            this.InsuranceContractService = InsuranceContractService;
            this.InsuranceContractProposalFilledFormJsonService = InsuranceContractProposalFilledFormJsonService;
            this.InsuranceContractProposalFilledFormUserService = InsuranceContractProposalFilledFormUserService;
            this.InsuranceContractProposalFilledFormValueService = InsuranceContractProposalFilledFormValueService;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
            this.UserService = UserService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.SmsValidationHistoryService = SmsValidationHistoryService;
        }

        public ApiResult Create(long? loginUserId, int? siteSettingId, IFormCollection form)
        {
            var contractInfo = new contractUserInput()
            {
                nationalCode = form.GetStringIfExist("nationalCode"),
                username = form.GetStringIfExist("username"),
                code = form.GetStringIfExist("code"),
            };

            (PageForm ppfObj, List<IdTitle> familyRelations, List<IdTitle> familyCTypes, IdTitle contract, string jsonConfigStr) =
                createValidation(loginUserId, siteSettingId, form, contractInfo);

            long newFormId = 0;

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    InsuranceContractProposalFilledForm newForm = createNewProposalFilledForm(siteSettingId, contract.id.ToIntReturnZiro(), loginUserId);
                    InsuranceContractProposalFilledFormJsonService.Create(newForm.Id, jsonConfigStr);
                    InsuranceContractProposalFilledFormUserService.Create(familyRelations, familyCTypes, newForm.Id, contractInfo, siteSettingId, "coverPersons", form, loginUserId, getArrItemsForText(form, "coverPersons", "description"));
                    InsuranceContractProposalFilledFormValueService.CreateByJsonConfig(ppfObj, newForm.Id, form);
                    //createUploadedFiles(siteSettingId, form, loginUserId, newForm.Id);

                    tr.Commit();

                    newFormId = newForm.Id;

                    SmsValidationHistoryService.ValidateBy(contractInfo.username.ToLongReturnZiro(), contractInfo.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
                    if (form.GetStringIfExist("reConfirmCode").ToIntReturnZiro() > 0)
                        SmsValidationHistoryService.ValidateBy(contractInfo.username.ToLongReturnZiro(), form.GetStringIfExist("reConfirmCode").ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -600, SmsValidationHistoryType.SMSForCreateContract);

                    UserNotifierService.Notify(loginUserId, UserNotificationType.CreateNewDamageClime, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(loginUserId, ProposalFilledFormUserType.CreateUser) }, newForm.Id, contract.title, siteSettingId, "/InsuranceContractBaseData/InsuranceContractProposalFilledForm/Detaile?id=" + newForm.Id);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), data = new { url = "/Contract/Detaile", id = newFormId } };
        }

        //private void createUploadedFiles(int? siteSettingId, IFormCollection form, long? loginUserId, long filledFormId)
        //{
        //    for (var i = 0; i < form.Files.Count; i++)
        //    {
        //        var file = form.Files[i];
        //        var currUserId = form.GetStringIfExist("coverPersons[" + i + "].familyMember").ToLongReturnZiro();
        //        long targetId = InsuranceContractProposalFilledFormUserService.GetBy(filledFormId, currUserId);
        //        UploadedFileService.UploadNewFile(FileType.InsuranceContractProposalFilledForm, file, loginUserId, siteSettingId, targetId, ".jpg,.png,.pdf,.doc,.docx,.xls", true);

        //    }
        //}

        private InsuranceContractProposalFilledForm createNewProposalFilledForm(int? siteSettingId, int contractId, long? loginUserId)
        {
            var newItem = new InsuranceContractProposalFilledForm()
            {
                SiteSettingId = siteSettingId.Value,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                InsuranceContractId = contractId
            };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem;
        }

        private (PageForm, List<IdTitle>, List<IdTitle>, IdTitle, string) createValidation(long? loginUserId, int? siteSettingId, IFormCollection form, contractUserInput contractInfo)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (!form.GetStringIfExist("username").IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(form.GetStringIfExist("code")))
                throw BException.GenerateNewException(BMessages.Invalid_Code);
            if (!form.GetStringIfExist("nationalCode").IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);

            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(contractInfo.username.ToLongReturnZiro(), contractInfo.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            string tempJsonconfig = InsuranceContractService.GetFormJsonConfig(contractInfo, siteSettingId);
            if (string.IsNullOrEmpty(tempJsonconfig))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            PageForm ppfObj = null;
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(tempJsonconfig); } catch (Exception) { throw; }
            if (ppfObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);

            var allCtrls = ppfObj.GetAllListOf<ctrl>();

            foreach (ctrl ctrl in allCtrls)
            {
                if (ctrl.isCtrlVisible(form, allCtrls) == true)
                {
                    ctrl.requiredValidationForCtrl(ctrl, form);
                    ctrl.reqularExperssionValidationCtrl(ctrl, form);
                    ctrl.validateBaseDataEnums(ctrl, form);
                    ctrl.validateAndUpdateValuesOfDS(ctrl, form);
                    ctrl.navionalCodeValidation(ctrl, form);
                    ctrl.validateAndUpdateCtrl(ctrl, form, allCtrls);
                    ctrl.validateAndUpdateMultiRowInputCtrl(ctrl, form, ppfObj);
                    ctrl.validateMinAndMaxDayForDateInput(ctrl, form);
                    ctrl.dublicateMapValueIfNeeded(ctrl, ppfObj, form);
                }
            }

            var familyRelations = InsuranceContractService.GetFamilyMemberList(contractInfo, siteSettingId);
            if (familyRelations == null || familyRelations.Count <= 1)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            familyRelations = familyRelations.Where(t => !string.IsNullOrEmpty(t.id)).ToList();
            var inputFamilyRelations = getArrItems(form, "coverPersons", "familyMember");
            if (inputFamilyRelations.Count == 0 || inputFamilyRelations.Count >= maxInputUsers)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (inputFamilyRelations.Count != inputFamilyRelations.Where(t => t.id.ToLongReturnZiro() > 0 && !string.IsNullOrEmpty(t.title)).Count())
                throw BException.GenerateNewException(BMessages.Please_Enter_All_Family_Memebers);
            foreach (var familyRelation in inputFamilyRelations)
                if (!familyRelations.Any(t => t.id == familyRelation.id && t.title == familyRelation.title))
                    throw BException.GenerateNewException(BMessages.Validation_Error);

            var familyCTypes = InsuranceContractService.GetContractTypeList(contractInfo, siteSettingId);
            if (familyCTypes == null || familyCTypes.Count <= 1)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            familyCTypes = familyCTypes.Where(t => !string.IsNullOrEmpty(t.id)).ToList();
            var inputFamilyCTypes = getArrItems(form, "coverPersons", "contractType");
            if (inputFamilyCTypes.Count == 0 || inputFamilyCTypes.Count >= maxInputUsers)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (inputFamilyCTypes.Count != inputFamilyCTypes.Where(t => t.id.ToLongReturnZiro() > 0 && !string.IsNullOrEmpty(t.title)).Count())
                throw BException.GenerateNewException(BMessages.Please_Enter_All_Family_Memebers);
            foreach (var inputFamilyCType in inputFamilyCTypes)
                if (!familyCTypes.Any(t => t.id == inputFamilyCType.id && t.title == inputFamilyCType.title))
                    throw BException.GenerateNewException(BMessages.Validation_Error);


            var contract = InsuranceContractService.GetIdTitleBy(contractInfo, siteSettingId);
            if (contract == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (!string.IsNullOrEmpty(form.GetStringIfExist("reConfirmCode")))
            {
                string isValidSMS = SmsValidationHistoryService.ValidatePreUsedBy(contractInfo.username.ToLongReturnZiro(), form.GetStringIfExist("reConfirmCode").ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -600, SmsValidationHistoryType.SMSForCreateContract);
                if (string.IsNullOrEmpty(isValidSMS))
                    throw BException.GenerateNewException(BMessages.Invalid_Code);
            }

            return (ppfObj, inputFamilyRelations, inputFamilyCTypes, contract, tempJsonconfig);
        }

        private List<IdTitle> getArrItems(IFormCollection form, string keyPart1, string keyPart2)
        {
            List<IdTitle> result = new List<IdTitle>();

            for (var i = 0; i < maxInputUsers; i++)
            {
                if (
                    !string.IsNullOrEmpty(form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2)) &&
                    !string.IsNullOrEmpty(form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2 + "_Title"))
                    )
                    result.Add(new IdTitle() { id = form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2), title = form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2 + "_Title") });
            }

            return result;
        }

        private List<string> getArrItemsForText(IFormCollection form, string keyPart1, string keyPart2)
        {
            List<string> result = new List<string>();

            for (var i = 0; i < maxInputUsers; i++)
            {
                if (
                    !string.IsNullOrEmpty(form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2))
                    )
                    result.Add(form.GetStringIfExist(keyPart1 + "[" + i + "]." + keyPart2));
            }

            return result;
        }

        public InsuranceContractProposalFilledFormDetaileVM Detaile(long? id, long? loginUserId, int? siteSettingId, bool ignoreLoginUserId = false, List<InsuranceContractProposalFilledFormType> status = null)
        {
            var result = new InsuranceContractProposalFilledFormDetaileVM();
            var foundItem = db.InsuranceContractProposalFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
               .Where(t => t.Id == id && (ignoreLoginUserId == true || t.CreateUserId == loginUserId) && t.IsDelete != true)
               .Where(t => status == null || t.InsuranceContractProposalFilledFormUsers.Any(tt => status.Contains(tt.Status)))
               .Select(t => new
               {
                   t.Id,
                   mobile = t.CreateUser.Mobile,
                   nationalcode = t.CreateUser.Nationalcode,
                   firstName = t.CreateUser.Firstname,
                   lastname = t.CreateUser.Lastname,
                   birthDate = t.CreateUser.BirthDate,
                   contractTitle = t.InsuranceContract.Title,
                   createDate = t.CreateDate,
                   t.SiteSettingId,
                   ProposalFormId = t.InsuranceContract.ProposalFormId,
                   values = t.InsuranceContractProposalFilledFormValues.Select(tt => new
                   {
                       tt.Value,
                       tt.InsuranceContractProposalFilledFormKey.Key
                   }).ToList()
               })
               .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundJson = InsuranceContractProposalFilledFormJsonService.GetBy(foundItem.Id);
            if (string.IsNullOrEmpty(foundJson))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            //foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);
            var fFoundSw = foundSw.FirstOrDefault();
            List<ContractProposalFilledFormPdfGroupVM> listGroup = new()
            {
                new ContractProposalFilledFormPdfGroupVM()
                {
                    title = "کاربر",
                    ContractProposalFilledFormPdfGroupItems = new List<ContractProposalFilledFormPdfGroupItem>()
                    {
                        new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "شماره همراه", value = foundItem.mobile },
                        new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد ملی", value = foundItem.nationalcode },
                        new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تاریخ تولد", value = foundItem.birthDate.ToFaDate()},
                        new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام و نام خانوادگی", value = foundItem.firstName + " " + foundItem.lastname }
                    }
                }
            };

            var allFamilyMembers = InsuranceContractProposalFilledFormUserService.GetByFormId(foundItem.Id);

            if (allFamilyMembers != null && allFamilyMembers.Count > 0)
            {
                var ContractProposalFilledFormPdfGroupItems = new List<ContractProposalFilledFormPdfGroupItem>();
                foreach (var member in allFamilyMembers)
                {
                    ContractProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نوع", value = member.typeTitle });
                    ContractProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام و نام خانوادگی", value = member.firstName + " " + member.lastName });
                    ContractProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-12 col-xs-12 col-lg-6", title = "نسبت", value = member.relation });
                    if (member.files != null && member.files.Count > 0)
                        foreach(var file in member.files)
                            ContractProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = file.title, value = file.url, isImage = true });
                    ContractProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-12 col-sm-12 col-xs-12 col-lg-12", title = "توضیحات", value = member.desc });
                }
                listGroup.Add(
                    new ContractProposalFilledFormPdfGroupVM()
                    {
                        title = "اطلاعات بیمه شده",
                        ContractProposalFilledFormPdfGroupItems = ContractProposalFilledFormPdfGroupItems
                    }
                    );
            }

            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<ContractProposalFilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();
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
                            //if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            //{
                            //    for (var i = 0; i < 20; i++)
                            //    {
                            //        bool hasAnyValue = false;
                            //        foreach (var subCtrl in ctrl.ctrls)
                            //        {
                            //            string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                            //            string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                            //            string subValue = foundItem.values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                            //            if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                            //            {
                            //                hasAnyValue = true;
                            //                ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                            //            }
                            //            else if (hasAnyValue == true && subCtrl.type == ctrlType.empty)
                            //                ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = subCtrl.parentCL });
                            //        }
                            //    }
                            //}
                            if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new ContractProposalFilledFormPdfGroupVM() { title = step.title, ContractProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }


            result.ProposalFilledFormPdfGroupVMs = listGroup;
            result.ppfTitle = foundItem.contractTitle;
            result.id = foundItem.Id + "";
            result.ppfCreateDate = foundItem.createDate.ToFaDate();
            result.createUserFullname = foundItem.firstName + " " + foundItem.lastname;

            return result;
        }

        public ApiResult Delete(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            var foundItem = db.InsuranceContractProposalFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id && t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status))
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsDelete = true;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public GridResultVM<InsuranceContractProposalFilledFormMainGridResultVM> GetList(InsuranceContractProposalFilledFormMainGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            searchInput = searchInput ?? new InsuranceContractProposalFilledFormMainGrid();

            var quiryResult = db.InsuranceContractProposalFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status));

            if (!string.IsNullOrEmpty(searchInput.createUserfullname))
                quiryResult = quiryResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUserfullname));
            if (!string.IsNullOrEmpty(searchInput.contractTitle))
                quiryResult = quiryResult.Where(t => t.InsuranceContract.Title.Contains(searchInput.contractTitle));
            if (!string.IsNullOrEmpty(searchInput.familyMemers))
                quiryResult = quiryResult.Where(t => t.InsuranceContractProposalFilledFormUsers.Any(tt => (tt.InsuranceContractUser.FirstName + " " + tt.InsuranceContractUser.LastName).Contains(searchInput.familyMemers)));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractProposalFilledFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    createUserfullname = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate,
                    contractTitle = t.InsuranceContract.Title,
                    familyMemers = t.InsuranceContractProposalFilledFormUsers.Select(tt => tt.InsuranceContractUser.FirstName + " " + tt.InsuranceContractUser.LastName).ToList(),
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractProposalFilledFormMainGridResultVM
                {
                    id = t.id,
                    row = ++row,
                    contractTitle = t.contractTitle,
                    createDate = t.createDate.ToString("hh:mm") + " " + t.createDate.ToFaDate(),
                    createUserfullname = t.createUserfullname,
                    status = status.GetEnumDisplayName(),
                    familyMemers = String.Join(",", t.familyMemers),
                    siteTitleMN2 = t.siteTitleMN2
                }).ToList(),
            };
        }

        public object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            input = input ?? new GlobalGridParentLong();

            var foundId = db.InsuranceContractProposalFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status))
                .SelectMany(t => t.InsuranceContractProposalFilledFormUsers)
                .Where(t => t.Status == status && t.Id == input.pKey)
                .Select(t => t.Id)
                .FirstOrDefault();

            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new
            {
                total = UploadedFileService.GetCountBy(input.pKey != null ? input.pKey.Value : -1, FileType.InsuranceContractProposalFilledForm, siteSettingId),
                data = UploadedFileService.GetListBy(input.pKey != null ? input.pKey.Value : -1, FileType.InsuranceContractProposalFilledForm, input.skip, input.take, siteSettingId)
            };
        }

        public object GetStatus(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            var foundItem = db.InsuranceContractProposalFilledForms
               .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
               .Where(t => t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Id == id && tt.Status == status))
               .Any();

            if (!foundItem)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new { status, id };
        }

        public object GetDescription(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            var foundItem = db.InsuranceContractProposalFilledForms
               .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
               .Where(t => t.IsDelete != true)
               .SelectMany(t => t.InsuranceContractProposalFilledFormUsers)
               .Where(tt => tt.Id == id && tt.Status == status)
               .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new { description = foundItem.Description };
        }
    }
}
