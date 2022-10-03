using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormService : ITenderFilledFormService
    {
        readonly TenderDBContext db = null;
        readonly ITenderFilledFormJsonService TenderFilledFormJsonService = null;
        readonly ITenderFilledFormsValueService TenderFilledFormsValueService = null;
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        readonly ITenderFilledFormPFService TenderFilledFormPFService = null;
        readonly IUserService UserService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TenderFilledFormService
            (
                TenderDBContext db,
                ITenderFilledFormJsonService TenderFilledFormJsonService,
                ITenderFilledFormsValueService TenderFilledFormsValueService,
                ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService,
                ITenderFilledFormPFService TenderFilledFormPFService,
                IUserService UserService,
                IUserNotifierService UserNotifierService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.TenderFilledFormJsonService = TenderFilledFormJsonService;
            this.TenderFilledFormsValueService = TenderFilledFormsValueService;
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
            this.TenderFilledFormPFService = TenderFilledFormPFService;
            this.UserService = UserService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId)
        {
            PageForm ppfObj = null;
            long newFormId = 0;
            string tempJsonFile = System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Tender", "Tender"));
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(tempJsonFile); } catch (Exception) { }

            createValidation(siteSettingId, form, ppfObj, loginUserId);

            List<int> TenderProposalFormJsonConfigIds = new List<int>();
            fillPPFIds(TenderProposalFormJsonConfigIds, form);

            if (TenderProposalFormJsonConfigIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Insurance);


            var configs = TenderProposalFormJsonConfigService.Validate(siteSettingId, TenderProposalFormJsonConfigIds);

            foreach (var config in configs)
            {
                PageForm tempJSConfig = null;
                try { tempJSConfig = JsonConvert.DeserializeObject<PageForm>(config.JsonConfig); } catch (Exception) { }
                createValidation(siteSettingId, form, tempJSConfig, loginUserId);
                config.PageForm = tempJSConfig;
            }


            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    TenderFilledForm newForm = create(siteSettingId, loginUserId);
                    TenderFilledFormJsonService.Create(newForm.Id, tempJsonFile, null);
                    TenderFilledFormsValueService.CreateByForm(newForm.Id, form, ppfObj, null);

                    foreach (var config in configs)
                    {
                        TenderFilledFormPFService.Create(newForm.Id, config.Id);
                        TenderFilledFormJsonService.Create(newForm.Id, config.JsonConfig, config.Id);
                        TenderFilledFormsValueService.CreateByForm(newForm.Id, form, config.PageForm, config.Id);
                    }

                    tr.Commit();

                    newFormId = newForm.Id;

                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), data = new { id = newFormId } };
        }

        private void fillPPFIds(List<int> tenderProposalFormJsonConfigIds, IFormCollection form)
        {
            if (tenderProposalFormJsonConfigIds != null && form != null)
                for (var i = 0; i < 50; i++)
                {
                    string curKey = "tenderInsurance[" + i + "].fid";
                    if (form.ContainsKey(curKey) && form.GetStringIfExist(curKey).ToIntReturnZiro() > 0)
                        tenderProposalFormJsonConfigIds.Add(form.GetStringIfExist(curKey).ToIntReturnZiro());
                }
        }

        private TenderFilledForm create(int? siteSettingId, long? loginUserId)
        {
            TenderFilledForm newItem = new TenderFilledForm()
            {
                CreateDate = DateTime.Now,
                SiteSettingId = siteSettingId.Value,
                UserId = loginUserId.Value,
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return newItem;
        }

        private void createValidation(int? siteSettingId, IFormCollection form, PageForm ppfObj, long? loginUserId)
        {
            if (form == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (ppfObj == null || ppfObj.panels == null || ppfObj.panels.Count == 0)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);


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
        }

        public TenderFilledFormPdfVM PdfDetailes(long id, int? siteSettingId, long? loginUserId)
        {
            TenderFilledFormPdfVM result = new TenderFilledFormPdfVM();

            var foundItem = db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id && (loginUserId == null || t.UserId == loginUserId))
                .Select(t => new
                {
                    id = t.Id,
                    createDate = t.CreateDate,
                    createUserFullname = t.User.Firstname + " " + t.User.Lastname,
                    ppfs = t.TenderFilledFormPFs.Select(tt => new
                    {
                        title = tt.TenderProposalFormJsonConfig.ProposalForm.Title,
                        id = tt.TenderProposalFormJsonConfigId
                    }).ToList(),
                    values = t.TenderFilledFormsValues.Select(tt => new
                    {
                        tt.Value,
                        Key = tt.TenderFilledFormKey.Key
                    }).ToList()
                })
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundJsons = TenderFilledFormJsonService.GetBy(id);
            if (foundJsons == null || foundJsons.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundJsons = foundJsons.OrderBy(t => t.TenderProposalFormJsonConfigId).ToList();
            List<TenderFilledFormPdfVMGroupVM> listGroup = new();
            foreach (var foundJson in foundJsons)
            {
                TenderFilledFormPdfVMGroupVM tempGroup = null;
                var curTitle = foundItem.ppfs.Where(t => t.id == foundJson.TenderProposalFormJsonConfigId).Select(t => t.title).FirstOrDefault();
                var curConfigId = foundItem.ppfs.Where(t => t.id == foundJson.TenderProposalFormJsonConfigId).Select(t => t.id).FirstOrDefault();
                if (!string.IsNullOrEmpty(curTitle))
                    tempGroup = new TenderFilledFormPdfVMGroupVM() { title = curTitle, id = id, configId = curConfigId };
                var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson.JsonConfig);
                if (jsonObj == null)
                    throw BException.GenerateNewException(BMessages.Json_Convert_Error);
                var foundSw = jsonObj.GetAllListOf<stepWizard>();
                if (foundSw == null || foundSw.Count == 0)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                var fFoundSw = foundSw.FirstOrDefault();
                foreach (var step in fFoundSw.steps)
                {
                    var allCtrls = step.GetAllListOf<ctrl>();
                    List<TenderFilledFormPdfVMGroupItem> ProposalFilledFormPdfGroupItems = new();
                    if (allCtrls != null && allCtrls.Count > 0)
                    {
                        foreach (var ctrl in allCtrls)
                        {
                            string title = !string.IsNullOrEmpty(ctrl.label) ? ctrl.label : ctrl.ph;
                            string value = foundItem.values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                            if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)))
                            {
                                if (ctrl.type == ctrlType.checkBox)
                                    title = "";
                                if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                    ProposalFilledFormPdfGroupItems.Add(new TenderFilledFormPdfVMGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                            }
                        }
                        if (ProposalFilledFormPdfGroupItems.Count > 0)
                        {
                            if (tempGroup != null)
                                tempGroup.TenderFilledFormPdfVMGroupVMs.Add(new TenderFilledFormPdfVMGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                            else
                                listGroup.Add(new TenderFilledFormPdfVMGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                        }
                    }
                }

                if (tempGroup != null)
                    listGroup.Add(tempGroup);
            }

            result.createDate = foundItem.createDate.ToFaDate();
            result.createUserFullname = foundItem.createUserFullname;
            result.ppfTitle = "مناقصه";
            result.id = foundItem.id;
            result.TenderFilledFormPdfVMGroupVMs = listGroup;

            return result;
        }

        public string GetDocument(long? id, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId)
        {
            var values = db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id && (loginUserId == null || t.UserId == loginUserId) && t.TenderFilledFormPFs.Any(tt => tt.TenderProposalFormJsonConfigId == tenderProposalFormJsonConfigId))
                .SelectMany(t => t.TenderFilledFormsValues)
                .Select(t => new { key = t.TenderFilledFormKey.Key, value = t.Value })
                .ToList();

            if (values == null || values.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            string pdfHtml = TenderProposalFormJsonConfigService.GetDocuemntHtml(tenderProposalFormJsonConfigId, siteSettingId);

            if (string.IsNullOrEmpty(pdfHtml))
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var value in values)
                pdfHtml = pdfHtml.Replace("{{" + value.key + "}}", value.value);

            pdfHtml = Regex.Replace(pdfHtml, @"{{[\w]+}}", "");

            return pdfHtml;
        }

        public GridResultVM<MyTenderFilledFormMainGridResultVM> GetListForWeb(MyTenderFilledFormMainGrid searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new MyTenderFilledFormMainGrid();

            var quiryResult = db.TenderFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.UserId == loginUserId);

            int row = searchInput.skip;

            return new GridResultVM<MyTenderFilledFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    insurances = t.TenderFilledFormPFs.Select(tt => tt.TenderProposalFormJsonConfig.ProposalForm.Title).ToList(),
                    createDate = t.CreateDate,
                    t.IsPublished,
                    hasIssueItem = t.TenderFilledFormIssues.Any(tt => tt.UserId == loginUserId),
                    t.OpenDate,
                    t.ProvinceId,
                    t.CityId,
                    cCount = t.TenderFilledFormValidCompanies.Count(),
                    pCount = t.TenderFilledFormPrices.Count(),
                    psCount = t.TenderFilledFormPrices.Count(t => t.IsConfirm == true),
                    issPublished = t.IsPublished
                })
                .ToList()
                .Select(t => new MyTenderFilledFormMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    insurances = getInsuranceByNumber(t.insurances),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                    isPub = t.IsPublished == true ? true : false,
                    status = getStatus(t.hasIssueItem, t.OpenDate, t.ProvinceId, t.CityId, t.cCount, t.pCount, t.psCount, t.issPublished)
                })
                .ToList()
            };
        }

        string getInsuranceByNumber(List<string> insurances)
        {
            string result = "";

            if (insurances != null && insurances.Count > 0)
                for (var i = 0; i < insurances.Count; i++)
                    result += (i + 1) + "- " + insurances[i] + (i < insurances.Count ? "<br />" : "");

            return result;
        }

        public GridResultVM<TenderFilledFormMainGridResultVM> GetList(TenderFilledFormMainGrid searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new TenderFilledFormMainGrid();

            var curDate = DateTime.Now;

            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);

            var quiryResult = db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => (t.CityId == null || t.CityId == cityid) && (t.ProvinceId == null || t.ProvinceId == province) && (!t.TenderFilledFormValidCompanies.Any() || t.TenderFilledFormValidCompanies.Any(tt => companyIds.Contains(tt.CompanyId))))
                ;

            quiryResult = quiryResult
                .Where(t => (t.OpenDate != null && t.OpenDate <= curDate && t.AvalibleDate != null && t.AvalibleDate >= curDate) || t.TenderFilledFormPrices.Any(t => t.UserId == loginUserId));

            if (!string.IsNullOrEmpty(searchInput.userfullname))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userfullname));
            if (!string.IsNullOrEmpty(searchInput.insurances))
                quiryResult = quiryResult.Where(t => t.TenderFilledFormPFs.Any(tt => tt.TenderProposalFormJsonConfig.ProposalForm.Title.Contains(searchInput.insurances)));
            if (searchInput.isPub != null)
                quiryResult = quiryResult.Where(t => t.IsPublished == searchInput.isPub);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<TenderFilledFormMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    insurances = t.TenderFilledFormPFs.Select(tt => tt.TenderProposalFormJsonConfig.ProposalForm.Title).ToList(),
                    createDate = t.CreateDate,
                    t.IsPublished,
                    userfullname = t.User.Firstname + " " + t.User.Lastname,
                    hasIssueItem = t.TenderFilledFormIssues.Any(tt => tt.UserId == loginUserId),
                    t.OpenDate,
                    t.ProvinceId,
                    t.CityId,
                    cCount = t.TenderFilledFormValidCompanies.Count(),
                    pCount = t.TenderFilledFormPrices.Count(),
                    psCount = t.TenderFilledFormPrices.Count(t => t.IsConfirm == true),
                    issPublished = t.IsPublished,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new TenderFilledFormMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    insurances = getInsuranceByNumber(t.insurances),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                    isPub = t.IsPublished == true ? true : false,
                    userfullname = t.userfullname,
                    status = getStatus(t.hasIssueItem, t.OpenDate, t.ProvinceId, t.CityId, t.cCount, t.pCount, t.psCount, t.issPublished),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        private string getStatus(bool hasIssueItem, DateTime? openDate, int? provinceId, int? cityId, int cCount, int pCount, int psCount, bool? issPublished)
        {
            string result = TenderStatus.Create.GetEnumDisplayName();

            if (hasIssueItem == true)
                result = TenderStatus.Issue.GetEnumDisplayName();
            else if (psCount > 0)
                result = TenderStatus.SelectPrice.GetEnumDisplayName();
            else if (pCount > 0)
                result = TenderStatus.FillPrice.GetEnumDisplayName();
            else if (issPublished == true)
                result = TenderStatus.Published.GetEnumDisplayName();
            else if (provinceId > 0 || cityId > 0 || cCount > 0)
                result = TenderStatus.FillCondation.GetEnumDisplayName();
            else if (openDate != null)
                result = TenderStatus.FillDates.GetEnumDisplayName();

            return result;
        }

        public ApiResult UpdateDatesForWeb(MyTenderFilledFormUpdateDateVM input, int? siteSettingId, long? loginUserId)
        {
            updateDateForWebValidation(input, siteSettingId, loginUserId);

            var foundItem = db.TenderFilledForms.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.IsPublished == true)
                throw BException.GenerateNewException(BMessages.It_Published_Already);

            foundItem.AvalibleDate = input.avalibleDate.ToEnDate();
            foundItem.OpenDate = input.openDate.ToEnDate();
            foundItem.PriceExpireDate = input.pExpirDate.ToEnDate();

            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.UpdateTenderDates, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(loginUserId, ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "تاریخ های مناقصه", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult UpdateDates(MyTenderFilledFormUpdateDateVM input, int? siteSettingId)
        {
            updateDateForWebValidation(input, siteSettingId, null, true);

            var foundItem = db.TenderFilledForms.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.AvalibleDate = input.avalibleDate.ToEnDate();
            foundItem.OpenDate = input.openDate.ToEnDate();
            foundItem.PriceExpireDate = input.pExpirDate.ToEnDate();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void updateDateForWebValidation(MyTenderFilledFormUpdateDateVM input, int? siteSettingId, long? loginUserId, bool ignoreLoginUserId = false)
        {
            var curDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (ignoreLoginUserId == false && loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (string.IsNullOrEmpty(input.openDate))
                throw BException.GenerateNewException(BMessages.Please_Select_OpenDate);
            if (input.openDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.openDate.ToEnDate().Value < curDate)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);

            if (string.IsNullOrEmpty(input.avalibleDate))
                throw BException.GenerateNewException(BMessages.Please_Select_CloseDate);
            if (input.avalibleDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.avalibleDate.ToEnDate().Value < curDate)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);

            if (string.IsNullOrEmpty(input.pExpirDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Price_ExpireDate);
            if (input.pExpirDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.pExpirDate.ToEnDate().Value < curDate)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);
        }

        public object GetDatesByForWeb(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledForms
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.UserId == loginUserId)
                .Select(t => new
                {
                    id = t.Id,
                    openDate = t.OpenDate,
                    avalibleDate = t.AvalibleDate,
                    pExpirDate = t.PriceExpireDate
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    openDate = t.openDate.ToFaDate(),
                    avalibleDate = t.avalibleDate.ToFaDate(),
                    pExpirDate = t.pExpirDate.ToFaDate()
                })
                .FirstOrDefault();
        }

        public object GetDatesBy(long? id, int? siteSettingId)
        {
            return db.TenderFilledForms
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    openDate = t.OpenDate,
                    avalibleDate = t.AvalibleDate,
                    pExpirDate = t.PriceExpireDate
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    openDate = t.openDate.ToFaDate(),
                    avalibleDate = t.avalibleDate.ToFaDate(),
                    pExpirDate = t.pExpirDate.ToFaDate()
                })
                .FirstOrDefault();
        }

        public object GetAgentAccessByForWeb(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledForms
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.UserId == loginUserId)
                .Select(t => new
                {
                    id = t.Id,
                    provinceId = t.ProvinceId,
                    cityId = t.CityId,
                    cIds = t.TenderFilledFormValidCompanies.Select(tt => tt.CompanyId).ToList()
                })
                .FirstOrDefault();
        }

        public object GetAgentAccessBy(long? id, int? siteSettingId)
        {
            return db.TenderFilledForms
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    provinceId = t.ProvinceId,
                    cityId = t.CityId,
                    cIds = t.TenderFilledFormValidCompanies.Select(tt => tt.CompanyId).ToList()
                })
                .FirstOrDefault();
        }

        public object UpdateAgentAccessForWeb(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId, long? loginUserId)
        {
            updateAgentAccessForWebValidation(input, siteSettingId, loginUserId);

            var foundItem = db.TenderFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.UserId == loginUserId && t.Id == input.id).Include(t => t.TenderFilledFormValidCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.IsPublished == true)
                throw BException.GenerateNewException(BMessages.It_Published_Already);

            if (foundItem.TenderFilledFormValidCompanies != null)
                foreach (var company in foundItem.TenderFilledFormValidCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            foundItem.CityId = input.cityId;
            foundItem.ProvinceId = input.provinceId;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new TenderFilledFormValidCompany()
                    {
                        CompanyId = cid,
                        TenderFilledFormId = foundItem.Id
                    }).State = EntityState.Added;

            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.UpdateTenderAccess, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(loginUserId, ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "شرایط مناقصه", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object UpdateAgentAccess(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId)
        {
            updateAgentAccessForWebValidation(input, siteSettingId, null, true);

            var foundItem = db.TenderFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).Include(t => t.TenderFilledFormValidCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.TenderFilledFormValidCompanies != null)
                foreach (var company in foundItem.TenderFilledFormValidCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            foundItem.CityId = input.cityId;
            foundItem.ProvinceId = input.provinceId;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new TenderFilledFormValidCompany()
                    {
                        CompanyId = cid,
                        TenderFilledFormId = foundItem.Id
                    }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void updateAgentAccessForWebValidation(MyTenderFilledFormUpdateAgentAccessVM input, int? siteSettingId, long? loginUserId, bool ignoreLoginUserId = false)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (ignoreLoginUserId == false && loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (input.provinceId.ToIntReturnZiro() <= 0 && input.cityId.ToIntReturnZiro() <= 0 && (input.cIds == null || input.cIds.Count == 0))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public object UpdatePublishForWeb(long? id, int? siteSettingId, long? loginUserId)
        {
            var foundItem = db.TenderFilledForms.Include(t => t.TenderFilledFormValidCompanies).Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.UserId == loginUserId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.AvalibleDate == null || foundItem.OpenDate == null || foundItem.PriceExpireDate == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Date);

            if (foundItem.CityId.ToIntReturnZiro() <= 0 && foundItem.ProvinceId.ToIntReturnZiro() <= 0 && (foundItem.TenderFilledFormValidCompanies == null || foundItem.TenderFilledFormValidCompanies.Count == 0))
                throw BException.GenerateNewException(BMessages.For_Publishe_You_Need_To_Enter_All_Other_Information);

            if (foundItem.IsPublished == true)
                throw BException.GenerateNewException(BMessages.It_Published_Already);

            foundItem.IsPublished = true;
            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.PublishTender, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(loginUserId, ProposalFilledFormUserType.OwnerUser) }, foundItem.Id, "انتشار مناقصه", siteSettingId, "/TenderAdmin/TenderFilledForm/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object UpdatePublish(long? id, int? siteSettingId)
        {
            var foundItem = db.TenderFilledForms.Include(t => t.TenderFilledFormValidCompanies).Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.AvalibleDate == null || foundItem.OpenDate == null || foundItem.PriceExpireDate == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Date);

            if (foundItem.CityId.ToIntReturnZiro() <= 0 && foundItem.ProvinceId.ToIntReturnZiro() <= 0 && (foundItem.TenderFilledFormValidCompanies == null || foundItem.TenderFilledFormValidCompanies.Count == 0))
                throw BException.GenerateNewException(BMessages.For_Publishe_You_Need_To_Enter_All_Other_Information);

            if (foundItem.IsPublished == null)
                foundItem.IsPublished = false;
            foundItem.IsPublished = !foundItem.IsPublished;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetInsuranceList(long? tenderFilledFormId, int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result
                .AddRange(db.TenderFilledForms
                                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                                .Where(t => t.Id == tenderFilledFormId)
                                .SelectMany(t => t.TenderFilledFormPFs)
                                .Select(t => new
                                {
                                    id = t.TenderProposalFormJsonConfigId,
                                    title = t.TenderProposalFormJsonConfig.ProposalForm.Title
                                }).ToList());

            return result;
        }

        public bool ExistByJCId(int? siteSettingId, long? id, int? tenderProposalFormJsonConfigId)
        {
            return db.TenderFilledForms.Any(t => t.SiteSettingId == siteSettingId && t.Id == id && t.TenderFilledFormPFs.Any(tt => tt.TenderProposalFormJsonConfigId == tenderProposalFormJsonConfigId));
        }

        public bool IsPublished(int? siteSettingId, long? id)
        {
            return db.TenderFilledForms.Any(t => t.SiteSettingId == siteSettingId && t.Id == id && t.IsPublished == true);
        }

        public bool ValidateCompany(int? siteSettingId, long? id, int? companyId)
        {
            return db.TenderFilledForms.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && t.TenderFilledFormValidCompanies.Any(tt => tt.CompanyId == companyId));
        }

        public bool ValidateProvinceAndCity(int? siteSettingId, long? id, int? provinceId, int? cityId)
        {
            var foundItem = db.TenderFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).Select(t => new { t.ProvinceId, t.CityId }).FirstOrDefault();

            if (foundItem != null)
            {
                if (foundItem.ProvinceId != null && foundItem.ProvinceId != provinceId)
                    return false;

                if (foundItem.CityId != null && foundItem.CityId != cityId)
                    return false;

                return true;
            }

            return false;
        }

        public bool ValidateOpenCloseDate(int? siteSettingId, long? id)
        {
            var curDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            return db.TenderFilledForms
                .Any(t => t.Id == id && t.SiteSettingId == siteSettingId && t.OpenDate != null && t.AvalibleDate != null && t.OpenDate <= curDate && t.AvalibleDate >= curDate);
        }

        public long? GetUserId(int? siteSettingId, long? id)
        {
            return db.TenderFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).Select(t => t.UserId).FirstOrDefault();
        }

        public int GetInsuranceCount(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TenderFilledForms
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.UserId == loginUserId)
                .SelectMany(t => t.TenderFilledFormPFs)
                .Count();
        }

        private void issueValidation(TenderFilledFormIssueVM input, int? siteSettingId, long? loginUserId)
        {
            var curDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.issueDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.issueDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.insuranceNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Number);
            if (input.minPic == null || input.minPic.Length == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (!db.TenderFilledForms.Any(t => t.Id == input.id && t.SiteSettingId == siteSettingId && t.TenderFilledFormPrices.Any(tt => tt.UserId == loginUserId && tt.IsConfirm == true)))
                throw BException.GenerateNewException(BMessages.You_CanNot_Do_This);
            if (input.issueDate.ToEnDate().Value < curDate)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Address_Length_Can_Not_Be_More_Then_4000);
        }
    }
}
