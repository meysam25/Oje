using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
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
    public class InsuranceContractProposalFilledFormUserService : IInsuranceContractProposalFilledFormUserService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IUserService UserService = null;
        readonly IInsuranceContractInsuranceContractTypeMaxPriceService InsuranceContractInsuranceContractTypeMaxPriceService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IUploadedFileService UploadedFileService = null;

        public InsuranceContractProposalFilledFormUserService(
                InsuranceContractBaseDataDBContext db,
                IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService,
                IUserNotifierService UserNotifierService,
                IUserService UserService,
                IInsuranceContractInsuranceContractTypeMaxPriceService InsuranceContractInsuranceContractTypeMaxPriceService,
                IInsuranceContractService InsuranceContractService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.InsuranceContractProposalFilledFormStatusLogService = InsuranceContractProposalFilledFormStatusLogService;
            this.UserNotifierService = UserNotifierService;
            this.UserService = UserService;
            this.InsuranceContractInsuranceContractTypeMaxPriceService = InsuranceContractInsuranceContractTypeMaxPriceService;
            this.InsuranceContractService = InsuranceContractService;
            this.UploadedFileService = UploadedFileService;
        }

        public void Create(List<IdTitle> familyRelations, List<IdTitle> familyCTypes, long insuranceContractProposalFilledFormId, contractUserInput contractInfo, int? siteSettingId, string fileName, IFormCollection form, long? loginUserId)
        {
            for (
                var i = 0;
                familyRelations != null && i < familyRelations.Count && familyCTypes != null && i < familyCTypes.Count;
                i++
                )
            {
                var newItem = new InsuranceContractProposalFilledFormUser()
                {
                    InsuranceContractProposalFilledFormId = insuranceContractProposalFilledFormId,
                    InsuranceContractTypeId = familyCTypes[i].id.ToIntReturnZiro(),
                    InsuranceContractUserId = familyRelations[i].id.ToLongReturnZiro(),
                    Status = InsuranceContractProposalFilledFormType.New
                };
                if (db.InsuranceContractProposalFilledFormUsers.Any(t => t.InsuranceContractTypeId == newItem.InsuranceContractTypeId && t.InsuranceContractUserId == newItem.InsuranceContractUserId && t.InsuranceContractProposalFilledFormId == newItem.InsuranceContractProposalFilledFormId))
                    throw BException.GenerateNewException(BMessages.Dublicate_Item);
                db.Entry(newItem).State = EntityState.Added;
                db.SaveChanges();

                var rds = InsuranceContractService.GetRequiredDocuments(contractInfo, newItem.InsuranceContractTypeId, siteSettingId);

                if (rds != null && rds.items != null && rds.items.Count > 0)
                {
                    for (var j = 0; j < rds.items.Count; j++)
                    {
                        var item = rds.items[j];
                        var file = form.Files[fileName + "[" + i + "]." + item.title.Replace(" ", "")];
                        if (item.isRequired == true && (file == null || file.Length == 0))
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Select_File_Format.GetEnumDisplayName(), item.title));
                        if (file != null && file.Length > 0)
                            UploadedFileService.UploadNewFile(FileType.InsuranceContractProposalFilledForm, file, loginUserId, siteSettingId, newItem.Id, ".jpg,.png,.pdf,.doc,.docx,.xls", true, null, item.title);
                    }
                }

            }
        }

        public ApiResult UpdateStatus(InsuranceContractProposalFilledFormChangeStatusVM input, int? siteSettingId, long? loginUserId, InsuranceContractProposalFilledFormType status)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.status == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Status);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            var foundItem =
            db.InsuranceContractProposalFilledFormUsers
            .Include(t => t.InsuranceContractProposalFilledForm).ThenInclude(t => t.InsuranceContract)
            .Where(t => t.Id == input.id && t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.InsuranceContractProposalFilledForm.IsDelete != true && t.Status == status)
            .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.Status == input.status)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (input.status == InsuranceContractProposalFilledFormType.Confirm)
                foundItem.ConfirmDate = DateTime.Now;

            foundItem.Status = input.status.Value;
            db.SaveChanges();

            InsuranceContractProposalFilledFormStatusLogService.Create(input.id, input.status, DateTime.Now, loginUserId, input.description);

            UserNotifierService.Notify(loginUserId, UserNotificationType.DamageClimeStatusChange, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(foundItem.InsuranceContractProposalFilledForm.CreateUserId, ProposalFilledFormUserType.CreateUser) }, foundItem.Id, foundItem?.InsuranceContractProposalFilledForm?.InsuranceContract?.Title, siteSettingId, "/InsuranceContractBaseData/InsuranceContractProposalFilledForm/Detaile?id=" + foundItem?.InsuranceContractProposalFilledForm?.Id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }


        public ApiResult UpdatePrice(InsuranceContractProposalFilledFormChangePriceVM input, int? siteSettingId, long? loginUserId, InsuranceContractProposalFilledFormType status)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

            var foundItem = db.InsuranceContractProposalFilledFormUsers
                .Include(t => t.InsuranceContractProposalFilledForm).ThenInclude(t => t.InsuranceContract)
                .Where(t => t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.InsuranceContractProposalFilledForm.IsDelete != true && t.Id == input.id && t.Status == status)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var maxPrice = InsuranceContractInsuranceContractTypeMaxPriceService.GetMaxPrice(siteSettingId, foundItem.InsuranceContractTypeId, foundItem.InsuranceContractProposalFilledForm?.InsuranceContractId);
            if (maxPrice.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxPrice);
            long usagePrice = getUsagePrice(foundItem.Id, foundItem.InsuranceContractProposalFilledForm?.InsuranceContractId, foundItem.InsuranceContractTypeId, siteSettingId, foundItem.InsuranceContractUserId);

            if (maxPrice < (usagePrice + input.price))
                throw BException.GenerateNewException(BMessages.Invalid_Price);

            foundItem.Price = input.price;
            db.SaveChanges();

            UserNotifierService.Notify(loginUserId, UserNotificationType.DamageClimeSetPrice, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(foundItem?.InsuranceContractProposalFilledForm?.CreateUserId, ProposalFilledFormUserType.CreateUser) }, foundItem.Id, foundItem?.InsuranceContractProposalFilledForm?.InsuranceContract?.Title, siteSettingId, "/InsuranceContractBaseData/InsuranceContractProposalFilledForm/Detaile?id=" + foundItem?.InsuranceContractProposalFilledForm.Id);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private long getUsagePrice(long id, int? insuranceContractId, int insuranceContractTypeId, int? siteSettingId, long insuranceContractUserId)
        {
            return db.InsuranceContractProposalFilledFormUsers
                .Where(t => t.Id != id && t.InsuranceContractProposalFilledForm.InsuranceContractId == insuranceContractId && t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.InsuranceContractTypeId == insuranceContractTypeId && t.InsuranceContractUserId == insuranceContractUserId)
                .Sum(t => t.Price != null ? t.Price.Value : 0);
        }

        public object GetPrice(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            return db.InsuranceContractProposalFilledFormUsers
                .Where(t => t.Id == id && t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.InsuranceContractProposalFilledForm.IsDelete != true && t.Status == status)
                .Select(t => new
                {
                    id = t.Id,
                    price = t.Price
                }).FirstOrDefault();
        }

        public object GetList(InsuranceContractProposalFilledFormDetailesMainGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            searchInput = searchInput ?? new InsuranceContractProposalFilledFormDetailesMainGrid();

            var quiryResult = db.InsuranceContractProposalFilledFormUsers.Where(t => t.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && t.Status == status && t.InsuranceContractProposalFilledFormId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (t.InsuranceContractUser.FirstName + " " + t.InsuranceContractUser.LastName).Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.type))
                quiryResult = quiryResult.Where(t => t.InsuranceContractType.Title.Contains(searchInput.type));
            if (!string.IsNullOrEmpty(searchInput.birthDate) && searchInput.birthDate.ToEnDate() != null)
            {
                var targetDate = searchInput.birthDate.ToEnDate().Value;
                quiryResult = quiryResult
                    .Where(t => t.InsuranceContractUser.BirthDate != null && t.InsuranceContractUser.BirthDate.Value.Year == targetDate.Year && t.InsuranceContractUser.BirthDate.Value.Month == targetDate.Month && t.InsuranceContractUser.BirthDate.Value.Day == targetDate.Day);
            }
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (searchInput.relation != null)
                quiryResult = quiryResult.Where(t => t.InsuranceContractUser.FamilyRelation == searchInput.relation);

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult.OrderByDescending(t => t.Id).Select(t => new
                {
                    id = t.Id,
                    userFullname = t.InsuranceContractUser.FirstName + " " + t.InsuranceContractUser.LastName,
                    type = t.InsuranceContractType.Title,
                    birthDate = t.InsuranceContractUser.BirthDate,
                    price = t.Price,
                    relation = t.InsuranceContractUser.FamilyRelation,
                    maxPrice = db.InsuranceContractInsuranceContractTypeMaxPrices.Where(tt => tt.InsuranceContractId == t.InsuranceContractProposalFilledForm.InsuranceContractId && t.InsuranceContractTypeId == tt.InsuranceContractTypeId).Select(tt => tt.MaxPrice).FirstOrDefault(),
                    usagePrice = db.InsuranceContractProposalFilledFormUsers.Where(tt => tt.InsuranceContractUserId == t.InsuranceContractUserId && tt.InsuranceContractTypeId == t.InsuranceContractTypeId).Sum(tt => tt.Price != null ? tt.Price.Value : 0)
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    t.userFullname,
                    t.type,
                    birthDate = t.birthDate.ToFaDate(),
                    price = t.price != null ? t.price.Value.ToString("###,###") : "",
                    relation = t.relation.GetEnumDisplayName(),
                    remindPrice = (t.maxPrice - t.usagePrice) > 0 ? (t.maxPrice - t.usagePrice).ToString("###,###") : ((t.maxPrice - t.usagePrice) + "")
                })
                .ToList()
            };
        }

        public long GetBy(long filledFormId, long currUserId)
        {
            return db.InsuranceContractProposalFilledFormUsers
                .Where(t => t.InsuranceContractUserId == currUserId && t.InsuranceContractProposalFilledFormId == filledFormId)
                .Select(t => t.Id)
                .FirstOrDefault();
        }

        public object GetAddress(long? id, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            return db.InsuranceContractProposalFilledForms
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.InsuranceContractProposalFilledFormUsers.Any(tt => status == tt.Status))
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    reciveLocation = t.ReciveLocation,
                    reciveDate = t.ReciveDate,
                    reciveTime = t.ReciveTime,
                    tell = t.ReciveTell,
                    address = t.ReciveAddress,
                    mapZoomRecivePlace = t.ReciveZoom,
                    mapLatRecivePlace = t.ReciveMapLocation != null ? (decimal?)t.ReciveMapLocation.X : null,
                    mapLonRecivePlace = t.ReciveMapLocation != null ? (decimal?)t.ReciveMapLocation.Y : null
                })
                .ToList().Select(t => new
                {
                    t.id,
                    t.reciveLocation,
                    reciveDate = t.reciveDate.ToFaDate(),
                    t.reciveTime,
                    t.tell,
                    t.address,
                    t.mapZoomRecivePlace,
                    t.mapLatRecivePlace,
                    t.mapLonRecivePlace
                })
                .FirstOrDefault();
        }
    }
}
