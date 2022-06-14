using Microsoft.AspNetCore.Http;
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
        readonly IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IUserService UserService = null;

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
                IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService,
                IUserNotifierService UserNotifierService,
                IUserService UserService
            )
        {
            this.db = db;
            this.InsuranceContractService = InsuranceContractService;
            this.InsuranceContractProposalFilledFormJsonService = InsuranceContractProposalFilledFormJsonService;
            this.InsuranceContractProposalFilledFormUserService = InsuranceContractProposalFilledFormUserService;
            this.InsuranceContractProposalFilledFormValueService = InsuranceContractProposalFilledFormValueService;
            this.UploadedFileService = UploadedFileService;
            this.InsuranceContractProposalFilledFormStatusLogService = InsuranceContractProposalFilledFormStatusLogService;
            this.UserNotifierService = UserNotifierService;
            this.UserService = UserService;
        }

        public ApiResult Create(long? loginUserId, int? siteSettingId, IFormCollection form)
        {
            var contractInfo = new contractUserInput()
            {
                birthDate = form.GetStringIfExist("birthDate"),
                contractCode = form.GetStringIfExist("contractCode").ToLongReturnZiro(),
                nationalCode = form.GetStringIfExist("nationalCode"),
                mobile = form.GetStringIfExist("mobile")
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
                    InsuranceContractProposalFilledFormUserService.Create(familyRelations, familyCTypes, newForm.Id, contractInfo, siteSettingId, "coverPersons", form, loginUserId);
                    InsuranceContractProposalFilledFormValueService.CreateByJsonConfig(ppfObj, newForm.Id, form);
                    //createUploadedFiles(siteSettingId, form, loginUserId, newForm.Id);

                    tr.Commit();

                    newFormId = newForm.Id;

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
            if (!form.GetStringIfExist("mobile").IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (!form.GetStringIfExist("nationalCode").IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
            if (string.IsNullOrEmpty(form.GetStringIfExist("birthDate")))
                throw BException.GenerateNewException(BMessages.Please_Enter_BirthDate);
            if (form.GetStringIfExist("birthDate").ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if ((DateTime.Now - form.GetStringIfExist("birthDate").ToEnDate().Value).TotalDays < year18DaysCount)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(form.GetStringIfExist("contractCode")))
                throw BException.GenerateNewException(BMessages.Invalid_Date);

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

        public InsuranceContractProposalFilledFormDetaileVM Detaile(long? id, long? loginUserId, int? siteSettingId, bool ignoreLoginUserId = false, List<InsuranceContractProposalFilledFormType> status = null)
        {
            var result = new InsuranceContractProposalFilledFormDetaileVM();
            var foundItem = db.InsuranceContractProposalFilledForms
               .Where(t => t.Id == id && (ignoreLoginUserId == true || t.CreateUserId == loginUserId) && t.SiteSettingId == siteSettingId && t.IsDelete != true)
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
                       Key = tt.InsuranceContractProposalFilledFormKey.Key
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
            List<ContractProposalFilledFormPdfGroupVM> listGroup = new();

            listGroup.Add(new ContractProposalFilledFormPdfGroupVM()
            {
                title = "کاربر",
                ContractProposalFilledFormPdfGroupItems = new List<ContractProposalFilledFormPdfGroupItem>()
                {
                    new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "شماره همراه", value = foundItem.mobile },
                    new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد ملی", value = foundItem.nationalcode },
                    new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تاریخ تولد", value = foundItem.birthDate.ToFaDate()},
                    new ContractProposalFilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام و نام خانوادگی", value = foundItem.firstName + " " + foundItem.lastname }
                }
            });

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
                            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            {
                                for (var i = 0; i < 20; i++)
                                {
                                    bool hasAnyValue = false;
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = foundItem.values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                        {
                                            hasAnyValue = true;
                                            ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                        }
                                        else if (hasAnyValue == true && subCtrl.type == ctrlType.empty)
                                            ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = subCtrl.parentCL });
                                    }
                                }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new ContractProposalFilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new ContractProposalFilledFormPdfGroupVM() { title = step.title, ContractProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }


            result.ProposalFilledFormPdfGroupVMs = listGroup;
            result.ppfTitle = foundItem.contractTitle;
            result.id = foundItem.SiteSettingId + "/" + foundItem.ProposalFormId + "/" + foundItem.Id;
            result.ppfCreateDate = foundItem.createDate.ToFaDate();
            result.createUserFullname = foundItem.firstName + " " + foundItem.lastname;

            return result;
        }

        public ApiResult Delete(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            var foundItem = db.InsuranceContractProposalFilledForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status)).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            foundItem.IsDelete = true;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public GridResultVM<InsuranceContractProposalFilledFormMainGridResultVM> GetList(InsuranceContractProposalFilledFormMainGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            searchInput = searchInput ?? new InsuranceContractProposalFilledFormMainGrid();

            var quiryResult = db.InsuranceContractProposalFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status));

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
                    familyMemers = t.InsuranceContractProposalFilledFormUsers.Select(tt => tt.InsuranceContractUser.FirstName + " " + tt.InsuranceContractUser.LastName).ToList()
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
                    familyMemers = String.Join(",", t.familyMemers)
                }).ToList(),
            };
        }

        public object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            input = input ?? new GlobalGridParentLong();

            var foundId = db.InsuranceContractProposalFilledFormUsers
                .Where(t => t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.Status == status && t.InsuranceContractProposalFilledForm.IsDelete != true && t.Id == input.pKey)
                .Select(t => t.Id)
                .FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new
            {
                total = UploadedFileService.GetCountBy(input.pKey != null ? input.pKey.Value : -1, FileType.InsuranceContractProposalFilledForm),
                data = UploadedFileService.GetListBy(input.pKey != null ? input.pKey.Value : -1, FileType.InsuranceContractProposalFilledForm, input.skip, input.take)
            };
        }

        public object GetStatus(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            var foundItem = db.InsuranceContractProposalFilledFormUsers.Where(t => t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.InsuranceContractProposalFilledForm.IsDelete != true && t.Id == id && t.Status== status).Any();
            if (!foundItem)
                throw BException.GenerateNewException(BMessages.Not_Found);
            return new { status = status, id = id };
        }
    }
}
