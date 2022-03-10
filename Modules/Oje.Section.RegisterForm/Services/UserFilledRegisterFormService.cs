using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Services
{
    public class UserFilledRegisterFormService : IUserFilledRegisterFormService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProvincService ProvincService = null;
        readonly ICityService CityService = null;
        readonly IUserFilledRegisterFormJsonService UserFilledRegisterFormJsonService = null;
        readonly IUserFilledRegisterFormValueService UserFilledRegisterFormValueService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly Interfaces.IUserService UserService = null;
        readonly IBankAccountFactorService BankAccountFactorService = null;

        public UserFilledRegisterFormService
            (
                RegisterFormDBContext db,
                IUserRegisterFormService UserRegisterFormService,
                IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService,
                ICompanyService CompanyService,
                IProvincService ProvincService,
                ICityService CityService,
                IUserFilledRegisterFormJsonService UserFilledRegisterFormJsonService,
                IUserFilledRegisterFormValueService UserFilledRegisterFormValueService,
                IUploadedFileService UploadedFileService,
                Interfaces.IUserService UserService,
                IBankAccountFactorService BankAccountFactorService
            )
        {
            this.db = db;
            this.UserFilledRegisterFormJsonService = UserFilledRegisterFormJsonService;
            this.UserRegisterFormService = UserRegisterFormService;
            this.UserRegisterFormRequiredDocumentService = UserRegisterFormRequiredDocumentService;
            this.CompanyService = CompanyService;
            this.ProvincService = ProvincService;
            this.CityService = CityService;
            this.UserFilledRegisterFormValueService = UserFilledRegisterFormValueService;
            this.UploadedFileService = UploadedFileService;
            this.UserService = UserService;
            this.BankAccountFactorService = BankAccountFactorService;
        }

        public object Create(int? siteSettingId, IFormCollection form, IpSections ipSections, long? parentUserId, long? userId)
        {
            createValidation(siteSettingId, form, ipSections);
            int formId = form.GetStringIfExist("fid").ToIntReturnZiro();
            var foundJsonFormStr = UserRegisterFormService.GetConfigJson(formId, siteSettingId);
            var allRequiredFileUpload = UserRegisterFormRequiredDocumentService.GetRequiredDocuments(formId, siteSettingId);
            long newFormId = 0;
            PageForm ppfObj = null;
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(foundJsonFormStr); }
            catch (Exception) { }
            // catch (Exception ex) { throw ex; }
            createCtrlValidation(form, ppfObj, allRequiredFileUpload, siteSettingId);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    UserFilledRegisterForm newItem = createNewItem(siteSettingId, ipSections, formId, getFormPrice(ppfObj, form), userId, form.GetStringIfExist("company"), form.GetStringIfExist("provinceId").ToIntReturnZiro(), form.GetStringIfExist("cityId").ToIntReturnZiro());
                    UserFilledRegisterFormJsonService.Create(foundJsonFormStr, newItem.Id);
                    UserFilledRegisterFormValueService.CreateByJsonConfig(ppfObj, newItem.Id, form);
                    createUploadedFiles(siteSettingId, form, newItem.Id);

                    tr.Commit();
                    newFormId = newItem.Id;

                    UserService.TemproryLogin(userId, siteSettingId, DateTime.Now.AddHours(2));
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), data = new { id = newFormId } };
        }

        private long getFormPrice(PageForm ppfObj, IFormCollection form)
        {
            long result = 0;
            var allCtrls = ppfObj.GetAllListOf<ctrl>();

            var foundPriceCtrl = allCtrls.Where(t => t.name == "price2").FirstOrDefault();
            if (foundPriceCtrl != null && foundPriceCtrl.isCtrlVisible(form, allCtrls) == true)
                result = form.GetStringIfExist(foundPriceCtrl.name).ToLongReturnZiro();

            foundPriceCtrl = allCtrls.Where(t => t.name == "price").FirstOrDefault();
            if (foundPriceCtrl != null && foundPriceCtrl.isCtrlVisible(form, allCtrls) == true)
                result = form.GetStringIfExist(foundPriceCtrl.name).ToLongReturnZiro();

            return result;
        }

        private void createUploadedFiles(int? siteSettingId, IFormCollection form, long userFilledRegisterFormId)
        {
            foreach (var file in form.Files)
            {
                UploadedFileService.UploadNewFile(FileType.RegisterUploadedDocuments, file, null, siteSettingId, userFilledRegisterFormId, ".jpg,.png,.pdf,.doc,.docx,.xls", true);
            }
        }

        private UserFilledRegisterForm createNewItem(int? siteSettingId, IpSections ipSections, int formId, long price, long? userId, string companyIds, int provinceId, int cityId)
        {
            var newItem = new UserFilledRegisterForm()
            {
                SiteSettingId = siteSettingId.Value,
                Ip1 = ipSections.Ip1,
                CreateDate = DateTime.Now,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                UserRegisterFormId = formId,
                UserId = userId,
                ProvinceId = (provinceId > 0 ? provinceId : null),
                CityId = (cityId > 0 ? cityId : null)
            };
            if (price > 0)
                newItem.Price = price;

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (!string.IsNullOrEmpty(companyIds))
            {
                var allParts = companyIds.Split(',').Where(t => t.ToIntReturnZiro() > 0).Select(t => t.ToIntReturnZiro()).ToList();
                if (allParts != null && allParts.Count > 0)
                {
                    foreach (var cid in allParts)
                        db.Entry(new UserFilledRegisterFormCompany() { CompanyId = cid, UserFilledRegisterFormId = newItem.Id }).State = EntityState.Added;

                    db.SaveChanges();
                }

            }

            return newItem;
        }

        private void createCtrlValidation(IFormCollection form, PageForm ppfObj, List<UserRegisterFormRequiredDocument> allRequiredFileUpload, int? siteSettingId)
        {
            if (ppfObj == null || ppfObj.panels == null || ppfObj.panels.Count == 0)
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
                    validateByUrl(ctrl, form);

                }
                validateFileUpload(ctrl, allRequiredFileUpload, form);
            }

            validateProvinceAndCity(allCtrls, form);
        }

        private void validateProvinceAndCity(List<ctrl> ctrls, IFormCollection form)
        {
            if (ctrls != null)
            {
                var foundProvince = ctrls.Where(t => !string.IsNullOrEmpty(t.dataurl) && t.dataurl.ToLower() == "/Core/BaseData/GetProvinceList".ToLower()).FirstOrDefault();
                var foundCity = ctrls.Where(t => !string.IsNullOrEmpty(t.dataurl) && t.dataurl.ToLower() == "/Core/BaseData/GetCityList2".ToLower()).FirstOrDefault();
                if (foundProvince != null && foundCity != null)
                {
                    var foundProvinceValue = form.GetStringIfExist(foundProvince.name);
                    var foundCityValue = form.GetStringIfExist(foundCity.name);
                    if (!string.IsNullOrEmpty(foundProvinceValue) && !string.IsNullOrEmpty(foundCityValue))
                    {
                        var foundProvinceObj = ProvincService.GetById(foundProvinceValue.ToIntReturnZiro());
                        if (foundProvinceObj == null)
                            throw BException.GenerateNewException(BMessages.Validation_Error);
                        var foundCityObj = CityService.GetById(foundCityValue.ToIntReturnZiro());
                        if (foundCityObj == null)
                            throw BException.GenerateNewException(BMessages.Validation_Error);

                        if (foundProvinceObj.Id != foundCityObj.ProvinceId)
                            throw BException.GenerateNewException(BMessages.Validation_Error);

                        foundProvince.defV = foundProvinceObj.Title;
                        foundCity.defV = foundCityObj.Title;
                    }
                }
            }

        }

        private void validateByUrl(ctrl ctrl, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(ctrl.dataurl))
            {
                if (ctrl.dataurl.ToLower() == "/Register/GetCompanyList".ToLower())
                {
                    var selectedValue = form.GetStringIfExist(ctrl.name);
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        var foundCompanyTitle = CompanyService.GetTitleById(selectedValue.ToIntReturnZiro());
                        if (string.IsNullOrEmpty(foundCompanyTitle))
                            throw BException.GenerateNewException(BMessages.Validation_Error);

                        ctrl.defV = foundCompanyTitle;
                    }
                }
            }
        }

        private void validateFileUpload(ctrl ctrl, List<UserRegisterFormRequiredDocument> allRequiredFileUpload, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dynamicFileUpload)
            {
                if (allRequiredFileUpload != null && allRequiredFileUpload.Count > 0)
                {
                    var allRequiredFiles = allRequiredFileUpload.Where(t => t.IsRequired == true).ToList();
                    foreach (var file in allRequiredFiles)
                        if (form.Files[file.Title.Replace(" ", "")] == null || form.Files[file.Title.Replace(" ", "")].Length == 0)
                            throw BException.GenerateNewException(string.Format(BMessages.Please_Select_X.GetEnumDisplayName(), file.Title));
                }
            }
        }

        private void createValidation(int? siteSettingId, IFormCollection form, IpSections ipSections)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (form == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!form.ContainsKey("fid"))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

            int formId = form.GetStringIfExist("fid").ToIntReturnZiro();
            if (formId <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (!UserRegisterFormService.Exist(formId, siteSettingId))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public userFilledRegisterFormDetailesVM PdfDetailes(long? id, int? siteSettingId, long? loginUserId)
        {
            var result = new userFilledRegisterFormDetailesVM();
            var foundItem = db.UserFilledRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (loginUserId == t.UserId || loginUserId == null) && t.UserId != null)
               .Where(t => t.Id == id)
               .Select(t => new
               {
                   t.Id,
                   t.Price,
                   t.CreateDate,
                   t.SiteSettingId,
                   t.PaymentTraceCode,
                   t.UserRegisterFormId,
                   PaymentUserId = t.UserRegisterForm.PaymentUserId,
                   ppfTitle = t.UserRegisterForm.Title,
                   createUserFullname = t.User.Firstname + " " + t.User.Lastname,
                   t.UserId,
                   values = t.UserFilledRegisterFormValues.Select(tt => new
                   {
                       tt.Value,
                       Key = tt.UserFilledRegisterFormKey.Key
                   }).ToList()
               })
               .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = UserFilledRegisterFormJsonService.GetBy(foundItem.Id);
            if (foundJson == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            var fFoundSw = foundSw.FirstOrDefault();
            List<userFilledRegisterFormDetailesGroupVM> listGroup = new();
            List<ProposalFilledFormPaymentVM> foundPaymentList = BankAccountFactorService.GetListBy(BankAccountFactorType.UserRegister, foundItem.Id, siteSettingId);
            if (foundPaymentList != null && foundPaymentList.Count > 0)
            {
                List<userFilledRegisterFormDetailesGroupItemVM> ProposalFilledFormPdfGroupPaymentItems = new();
                foreach (var item in foundPaymentList)
                {
                    ProposalFilledFormPdfGroupPaymentItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام حساب", value = item.fullName });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "مبلغ", value = item.price.ToString("###,###") + " ریال" });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تاریخ", value = item.payDate.ToFaDate() + " " + item.payDate.ToString("hh:mm") });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد پیگیری", value = item.traceCode });
                }
                listGroup.Add(new userFilledRegisterFormDetailesGroupVM() { title = "وضعیت پرداخت", items = ProposalFilledFormPdfGroupPaymentItems });
            }
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<userFilledRegisterFormDetailesGroupItemVM> ProposalFilledFormPdfGroupItems = new();
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
                                            ProposalFilledFormPdfGroupItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                        }
                                    }
                                }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new userFilledRegisterFormDetailesGroupItemVM() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new userFilledRegisterFormDetailesGroupVM() { title = step.title, items = ProposalFilledFormPdfGroupItems });
                }
            }


            result.proposalFilledFormId = foundItem.Id;
            result.groups = listGroup;
            result.createUserFullname = foundItem.createUserFullname;
            result.traceCode = foundItem.PaymentTraceCode;
            result.ppfTitle = foundItem.ppfTitle;
            result.id = foundItem.SiteSettingId + "/" + foundItem.UserRegisterFormId + "/" + foundItem.Id;
            result.price = foundItem.Price;
            result.ppfCreateDate = foundItem.CreateDate.ToFaDate();
            result.paymentUserId = foundItem.PaymentUserId;
            Company foundCompany = CompanyService.GetByUserFilledRegisterFormId(foundItem.Id);
            if (foundCompany != null)
            {
                result.companyTitle = foundCompany.Title;
                result.companyImage = GlobalConfig.FileAccessHandlerUrl + foundCompany.Pic;
            }

            return result;
        }

        public GridResultVM<UserFilledRegisterFormMainGridResultVM> GetList(UserFilledRegisterFormMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.UserFilledRegisterForms.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.username))
                qureResult = qureResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => tt.UserFilledRegisterFormKey.Key == "mobile" && tt.Value.Contains(searchInput.username)));
            if (!string.IsNullOrEmpty(searchInput.firstname))
                qureResult = qureResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => tt.UserFilledRegisterFormKey.Key == "firstName" && tt.Value.Contains(searchInput.username)));
            if (!string.IsNullOrEmpty(searchInput.lastname))
                qureResult = qureResult.Where(t => t.UserFilledRegisterFormValues.Any(tt => tt.UserFilledRegisterFormKey.Key == "lastName" && tt.Value.Contains(searchInput.username)));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.formTitle))
                qureResult = qureResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.formTitle));
            if (searchInput.price != null)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (searchInput.isPayed != null && searchInput.isPayed == true)
                qureResult = qureResult.Where(t => !string.IsNullOrEmpty(t.PaymentTraceCode));
            if (searchInput.isPayed != null && searchInput.isPayed == false)
                qureResult = qureResult.Where(t => string.IsNullOrEmpty(t.PaymentTraceCode));
            if (!string.IsNullOrEmpty(searchInput.traceCode))
                qureResult = qureResult.Where(t => t.PaymentTraceCode == searchInput.traceCode);
            if(searchInput.isDone != null)
                qureResult = qureResult.Where(t => t.IsDone == searchInput.isDone);

            int row = searchInput.skip;

            return new GridResultVM<UserFilledRegisterFormMainGridResultVM>()
            {
                total = qureResult.LongCount(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    username = t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "mobile").Select(tt => tt.Value).FirstOrDefault(),
                    firstname = t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "firstName").Select(tt => tt.Value).FirstOrDefault(),
                    lastname = t.UserFilledRegisterFormValues.Where(tt => tt.UserFilledRegisterFormKey.Key == "lastName").Select(tt => tt.Value).FirstOrDefault(),
                    createDate = t.CreateDate,
                    formTitle = t.UserRegisterForm.Title,
                    price = t.Price,
                    isPayed = !string.IsNullOrEmpty(t.PaymentTraceCode),
                    traceCode = t.PaymentTraceCode,
                    t.IsDone
                })
                .ToList()
                .Select(t => new UserFilledRegisterFormMainGridResultVM
                {
                    id = t.id,
                    row = ++row,
                    firstname = t.firstname,
                    traceCode = t.traceCode,
                    formTitle = t.formTitle,
                    isPayed = t.isPayed == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    lastname = t.lastname,
                    price = t.price > 0 ? t.price.Value.ToString("###,###") : "",
                    username = t.username,
                    createDate = t.createDate.ToFaDate(),
                    isDone = t.IsDone == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public object Delete(long? id, int? siteSettingId)
        {
            var foundItem =
                db.UserFilledRegisterForms
                .Include(t => t.UserFilledRegisterFormJsons)
                .Include(t => t.UserFilledRegisterFormValues)
                .Include(t => t.UserFilledRegisterFormCompanies)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.UserFilledRegisterFormJsons != null && foundItem.UserFilledRegisterFormJsons.Count > 0)
                foreach (var item in foundItem.UserFilledRegisterFormJsons)
                    db.Entry(item).State = EntityState.Deleted;

            if (foundItem.UserFilledRegisterFormValues != null && foundItem.UserFilledRegisterFormValues.Count > 0)
                foreach (var item in foundItem.UserFilledRegisterFormValues)
                    db.Entry(item).State = EntityState.Deleted;

            if (foundItem.UserFilledRegisterFormCompanies != null && foundItem.UserFilledRegisterFormCompanies.Count > 0)
                foreach (var item in foundItem.UserFilledRegisterFormCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object CreateNewUser(long? id, int? siteSettingId, long? parentId, List<int> roleIds)
        {
            var foundItem = db.UserFilledRegisterForms
                .Include(t => t.UserFilledRegisterFormValues).ThenInclude(t => t.UserFilledRegisterFormKey)
                .Include(t => t.UserFilledRegisterFormCompanies)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundItem.IsDone == true)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            var result = UserService.CreateNewUser(foundItem, siteSettingId, parentId, roleIds);
            if(result.isSuccess == true)
            {
                foundItem.IsDone = true;
                db.SaveChanges();
            }

            return result;
        }

        public object GetUploadImages(GlobalGridParentLong input, int? siteSettingId)
        {
            if (input == null)
                input = new GlobalGridParentLong();
            var foundItemId =
                db.UserFilledRegisterForms.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.pKey)
               .Select(t => t.Id)
                .FirstOrDefault();

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, FileType.RegisterUploadedDocuments),
                data = UploadedFileService.GetListBy(foundItemId, FileType.RegisterUploadedDocuments, input.skip, input.take)
            };
        }
    }
}
