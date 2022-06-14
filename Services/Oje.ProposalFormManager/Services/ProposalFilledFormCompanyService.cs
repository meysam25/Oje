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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormCompanyService : IProposalFilledFormCompanyService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService = null;
        readonly IUserService UserService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IProposalFilledFormUseService ProposalFilledFormUseService = null;
        readonly IUserNotifierService UserNotifierService = null;
        public ProposalFilledFormCompanyService(
            ProposalFormDBContext db,
            IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
            IUserService UserService,
            IProposalFilledFormUseService ProposalFilledFormUseService,
            IUploadedFileService UploadedFileService,
            IUserNotifierService UserNotifierService
            )
        {
            this.db = db;
            this.ProposalFilledFormAdminBaseQueryService = ProposalFilledFormAdminBaseQueryService;
            this.UserService = UserService;
            this.UploadedFileService = UploadedFileService;
            this.ProposalFilledFormUseService = ProposalFilledFormUseService;
            this.UserNotifierService = UserNotifierService;
        }

        public void Create(long inquiryId, int? siteSettingId, long proposalFilledFormId, long proposalFilledFormPrice, int companyId, bool isSelected, long? loginUserId)
        {
            if (inquiryId > 0 && companyId > 0)
            {
                db.Entry(new ProposalFilledFormCompany()
                {
                    CompanyId = companyId,
                    ProposalFilledFormId = proposalFilledFormId,
                    Price = proposalFilledFormPrice,
                    IsSelected = isSelected,
                    CreateDate = DateTime.Now,
                    CreateUserId = loginUserId
                }).State = EntityState.Added;
                db.SaveChanges();

            }
        }

        public void Create(string companyIds, long proposalFilledFormId, long? loginUserId)
        {
            if (string.IsNullOrEmpty(companyIds))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            List<string> allComIdsStr = companyIds.Split(',').ToList();
            List<int> allComIdsInt = allComIdsStr.Where(t => t.ToIntReturnZiro() > 0).Select(t => t.ToIntReturnZiro()).ToList();
            if (allComIdsStr.Count != allComIdsInt.Count)
                throw BException.GenerateNewException(BMessages.Selected_Company_Is_Not_Valid);
            if (db.Companies.Count(t => t.IsActive == true && allComIdsInt.Contains(t.Id)) != allComIdsInt.Count)
                throw BException.GenerateNewException(BMessages.Selected_Company_Is_Not_Valid);
            foreach (var cid in allComIdsInt)
                db.Entry(new ProposalFilledFormCompany()
                {
                    CompanyId = cid,
                    ProposalFilledFormId = proposalFilledFormId,
                    Price = 0,
                    CreateDate = DateTime.Now,
                    CreateUserId = loginUserId
                }).State = EntityState.Added;
            db.SaveChanges();
        }

        public ApiResult Create(CreateUpdateProposalFilledFormCompanyPrice input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            createPriceCompanyValidation(input, siteSettingId, userId);
            var foundPPFId = ProposalFilledFormAdminBaseQueryService
               .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
               .Where(t => t.Id == input.pKey).Select(t => new { t.GlobalInqueryId, t.Id }).FirstOrDefault();
            if (foundPPFId == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (foundPPFId.GlobalInqueryId.ToLongReturnZiro() > 0)
                throw BException.GenerateNewException(BMessages.You_Can_Not_Edit_Company_Of_ProposalFilledForm_With_Inquiry);

            var newItem = new ProposalFilledFormCompany()
            {
                CompanyId = input.companyId.ToIntReturnZiro(),
                ProposalFilledFormId = input.pKey.ToLongReturnZiro(),
                Price = input.price.ToLongReturnZiro(),
                IsSelected = false,
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            if (input.mainFile != null && input.mainFile.Length > 0)
                newItem.MainFileUrl = UploadedFileService.UploadNewFile(FileType.CompanyPrice, input.mainFile, userId, siteSettingId, input.pKey, ".png,.jpg,.jpeg,.doc,docx,.pdf", true, input.companyId + "_" + input.pKey);

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormCompanyChanged, ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.pKey.ToLongReturnZiro()), input.pKey, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.pKey);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createPriceCompanyValidation(CreateUpdateProposalFilledFormCompanyPrice input, int? siteSettingId, long? userId, bool isEdit = false)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (userId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.companyId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (isEdit == false && input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (isEdit == false && db.ProposalFilledFormCompanies.Any(t => t.CompanyId == input.companyId && t.ProposalFilledFormId == input.pKey))
                throw BException.GenerateNewException(BMessages.Please_Use_Edit_Button);
            if (isEdit == true)
            {
                if (string.IsNullOrEmpty(input.id) || input.id.IndexOf("_") == -1)
                    throw BException.GenerateNewException(BMessages.Not_Found);
                long ppfId = input.id.Split('_')[1].ToLongReturnZiro();
                int cId = input.id.Split('_')[0].ToIntReturnZiro();
                if (ppfId.ToLongReturnZiro() <= 0 || cId.ToLongReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.Not_Found);
            }
        }

        public void CreateUpdate(CreateUpdateProposalFilledFormCompany input, long? loginUserId)
        {
            var allCompay = db.ProposalFilledFormCompanies.Where(t => t.ProposalFilledFormId == input.id).ToList();
            List<int> canNotBeRemoved = new List<int>();
            foreach (var company in allCompay)
                if (company.IsSelected != true)
                    db.Entry(company).State = EntityState.Deleted;
                else
                    canNotBeRemoved.Add(company.CompanyId);

            input.cIds = input.cIds.Where(t => !canNotBeRemoved.Contains(t)).ToList();
            if (input.cIds.Count > 0)
                Create(String.Join(",", input.cIds), input.id.ToLongReturnZiro(), loginUserId);
            else
                db.SaveChanges();
        }

        public ApiResult Delete(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (string.IsNullOrEmpty(id) || id.IndexOf("_") == -1)
                throw BException.GenerateNewException(BMessages.Not_Found);
            long ppfId = id.Split('_')[1].ToLongReturnZiro();
            int cId = id.Split('_')[0].ToIntReturnZiro();
            if (ppfId.ToLongReturnZiro() <= 0 || cId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundItem =
                ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == ppfId)
                .SelectMany(t => t.ProposalFilledFormCompanies)
                .Where(t => t.CompanyId == cId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundItem.IsSelected == true)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public GridResultVM<ProposalFilledFormCompanyPriceMainGridResultVM> GetList(ProposalFilledFormCompanyPriceMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormCompanyPriceMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == searchInput.pKey)
                .SelectMany(t => t.ProposalFilledFormCompanies);

            if (searchInput.cId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CompanyId == searchInput.cId);
            if (searchInput.price != null)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => t.CreateUserId > 0 && (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.updateUser))
                qureResult = qureResult.Where(t => t.UpdateUserId > 0 && (t.UpdateUser.Firstname + " " + t.UpdateUser.Lastname).Contains(searchInput.updateUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate != null && t.CreateDate.Value.Year == targetDate.Year && t.CreateDate.Value.Month == targetDate.Month && t.CreateDate.Value.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.updateDate) && searchInput.updateDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.updateDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.UpdateDate != null && t.UpdateDate.Value.Year == targetDate.Year && t.UpdateDate.Value.Month == targetDate.Month && t.UpdateDate.Value.Day == targetDate.Day);
            }
            if (searchInput.isSelected != null)
                qureResult = qureResult.Where(t => t.IsSelected == searchInput.isSelected);

            var userAllCompanies = UserService.GetUserCompanies(userId);
            if (userAllCompanies.Count > 0)
                qureResult = qureResult.Where(t => userAllCompanies.Contains(t.CompanyId));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFilledFormCompanyPriceMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.CompanyId).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    companyId = t.CompanyId,
                    ppfId = t.ProposalFilledFormId,
                    cId = t.Company.Title,
                    price = t.Price,
                    createUser = t.CreateUserId > 0 ? t.CreateUser.Firstname + " " + t.CreateUser.Lastname : "",
                    createDate = t.CreateDate,
                    updateUser = t.UpdateUserId > 0 ? t.UpdateUser.Firstname + " " + t.UpdateUser.Lastname : "",
                    updateDate = t.UpdateDate,
                    isSelected = t.IsSelected
                })
                .ToList()
                .Select(t => new ProposalFilledFormCompanyPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.companyId + "_" + t.ppfId,
                    cId = t.cId,
                    price = t.price != 0 ? t.price.ToString("###,###") : "0",
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    updateUser = t.updateUser,
                    updateDate = t.updateDate.ToFaDate(),
                    isSelected = t.isSelected == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    isSelectedBool = t.isSelected
                })
                .ToList()
            };
        }

        public object GetBy(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (string.IsNullOrEmpty(id) || id.IndexOf("_") == -1)
                throw BException.GenerateNewException(BMessages.Not_Found);
            long ppfId = id.Split('_')[1].ToLongReturnZiro();
            int cId = id.Split('_')[0].ToIntReturnZiro();
            if (ppfId.ToLongReturnZiro() <= 0 || cId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundItem =
                ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == ppfId)
                .SelectMany(t => t.ProposalFilledFormCompanies)
                .Where(t => t.CompanyId == cId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return new
            {
                id = id,
                companyId = foundItem.CompanyId,
                price = foundItem.Price,
                mainFile_address = !string.IsNullOrEmpty(foundItem.MainFileUrl) ? GlobalConfig.FileAccessHandlerUrl + foundItem.MainFileUrl : ""
            };
        }

        public object Update(CreateUpdateProposalFilledFormCompanyPrice input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            createPriceCompanyValidation(input, siteSettingId, userId, true);
            if (string.IsNullOrEmpty(input.id) || input.id.IndexOf("_") == -1)
                throw BException.GenerateNewException(BMessages.Not_Found);
            long ppfId = input.id.Split('_')[1].ToLongReturnZiro();
            int cId = input.id.Split('_')[0].ToIntReturnZiro();
            if (ppfId.ToLongReturnZiro() <= 0 || cId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundPPFId = ProposalFilledFormAdminBaseQueryService
               .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
               .Where(t => t.Id == ppfId).Select(t => new { t.GlobalInqueryId, t.Id }).FirstOrDefault();
            if (foundPPFId == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (foundPPFId.GlobalInqueryId.ToLongReturnZiro() > 0)
                throw BException.GenerateNewException(BMessages.You_Can_Not_Edit_Company_Of_ProposalFilledForm_With_Inquiry);

            var editItem = db.ProposalFilledFormCompanies.Where(t => t.ProposalFilledFormId == ppfId && t.CompanyId == cId).FirstOrDefault();
            if (editItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            editItem.Price = input.price.ToLongReturnZiro();
            editItem.UpdateUserId = userId;
            editItem.UpdateDate = DateTime.Now;

            if (input.mainFile != null && input.mainFile.Length > 0)
                editItem.MainFileUrl = UploadedFileService.UploadNewFile(FileType.CompanyPrice, input.mainFile, userId, siteSettingId, ppfId, ".png,.jpg,.jpeg,.doc,docx,.pdf", true, cId + "_" + ppfId);

            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormCompanyChanged, ProposalFilledFormUseService.GetProposalFilledFormUserIds(ppfId), ppfId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + ppfId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object Select(string id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (string.IsNullOrEmpty(id) || id.IndexOf("_") == -1)
                throw BException.GenerateNewException(BMessages.Not_Found);
            long ppfId = id.Split('_')[1].ToLongReturnZiro();
            int cId = id.Split('_')[0].ToIntReturnZiro();
            if (ppfId.ToLongReturnZiro() <= 0 || cId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundItem =
                ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, status)
                .Where(t => t.Id == ppfId)
                .Include(t => t.ProposalFilledFormCompanies)
                .FirstOrDefault();
            if (foundItem == null || foundItem.ProposalFilledFormCompanies == null || foundItem.ProposalFilledFormCompanies.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var item in foundItem.ProposalFilledFormCompanies)
                item.IsSelected = false;

            var foundCompanny = foundItem.ProposalFilledFormCompanies.Where(t => t.CompanyId == cId).FirstOrDefault();
            if (foundCompanny == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundCompanny.Price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            foundCompanny.IsSelected = true;
            foundItem.Price = foundCompanny.Price;

            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormPriceSelected, ProposalFilledFormUseService.GetProposalFilledFormUserIds(ppfId.ToLongReturnZiro()), ppfId, "", siteSettingId, "/ProposalFilledForm"+ ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + ppfId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public Company GetSelectedBy(long proposalFilledFormId)
        {
            return db.ProposalFilledFormCompanies.Where(t => t.ProposalFilledFormId == proposalFilledFormId && t.IsSelected == true).Select(t => t.Company).FirstOrDefault();
        }

        public bool IsSelectedBy(long proposalFilledFormId)
        {
            return db.ProposalFilledFormCompanies.Any(t => t.IsSelected == true);
        }
    }
}
