using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class MyFilledContractService : IMyFilledContractService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;

        public MyFilledContractService
            (
                InsuranceContractBaseDataDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public object GetAddress(long? id, int? siteSettingId, long? userId, List<InsuranceContractProposalFilledFormType> validStatus)
        {
            return db.InsuranceContractProposalFilledForms
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.CreateUserId == userId && validStatus.Contains(t.Status))
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

        public object GetList(MyFilledContractMainGrid searchInput, long? loginUserId, int? siteSettingId, List<InsuranceContractProposalFilledFormType> status)
        {
            searchInput = searchInput ?? new MyFilledContractMainGrid();

            var qureResult = db.InsuranceContractProposalFilledForms.Where(t => t.CreateUserId == loginUserId && t.SiteSettingId == siteSettingId && status.Contains(t.Status) && t.IsDelete != true);

            if (!string.IsNullOrEmpty(searchInput.contractTitle))
                qureResult = qureResult.Where(t => t.InsuranceContract.Title.Contains(searchInput.contractTitle));
            if (!string.IsNullOrEmpty(searchInput.familyMemebers))
                qureResult = qureResult.Where(t => t.InsuranceContractProposalFilledFormUsers.Any(tt => (tt.InsuranceContractUser.FirstName + " " + tt.InsuranceContractUser.LastName).Contains(searchInput.familyMemebers)));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    contractTitle = t.InsuranceContract.Title,
                    createDate = t.CreateDate,
                    familyMemebers = t.InsuranceContractProposalFilledFormUsers.Select(tt => tt.InsuranceContractUser.FirstName + " " + tt.InsuranceContractUser.LastName).ToList(),
                    status = t.Status
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    familyMemebers = string.Join(",", t.familyMemebers),
                    createDate = t.createDate.ToFaDate(),
                    t.contractTitle,
                    status = t.status.GetEnumDisplayName()
                }).ToList()
            };
        }

        public object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> status)
        {
            input = input ?? new GlobalGridParentLong();

            var foundItemId = db.InsuranceContractProposalFilledForms
                    .Where(t =>
                        t.Id == input.pKey && t.SiteSettingId == siteSettingId && t.CreateUserId == loginUserId &&
                        t.IsDelete != true && status.Contains(t.Status))
                    .Select(t => t.Id)
                    .FirstOrDefault();

            if (foundItemId <= 0)
                foundItemId = -1;

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, FileType.InsuranceContractProposalFilledForm),
                data = UploadedFileService.GetListBy(foundItemId, FileType.InsuranceContractProposalFilledForm, input.skip, input.take)
            };
        }

        public object UpdateAddress(MyFilledContractAddressVM input, long? userId, int? siteSettingId, List<InsuranceContractProposalFilledFormType> validStatus)
        {
            UpdateAddressValidation(input, userId, siteSettingId);

            var foundItem = db.InsuranceContractProposalFilledForms.Where(t => t.IsDelete != true && t.Id == input.id && t.CreateUserId == userId && t.SiteSettingId == siteSettingId && validStatus.Contains(t.Status)).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.ReciveLocation = input.reciveLocation;
            foundItem.ReciveDate = input.reciveDate.ToEnDate().Value;
            foundItem.ReciveTime = input.reciveTime;
            foundItem.ReciveTell = input.tell;
            foundItem.ReciveAddress = input.address;
            foundItem.ReciveZoom = input.mapZoomRecivePlace;
            if (input.mapLatRecivePlace != null && input.mapLonRecivePlace != null)
                foundItem.ReciveMapLocation = new Point(input.mapLatRecivePlace.ToDoubleReturnNull().Value, input.mapLonRecivePlace.ToDoubleReturnNull().Value) { SRID = 4326 };
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void UpdateAddressValidation(MyFilledContractAddressVM input, long? userId, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.reciveLocation == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Recive_Location);
            if (input.id.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.reciveLocation == ContractLocation.MyLocation)
            {
                if (string.IsNullOrEmpty(input.tell))
                    throw BException.GenerateNewException(BMessages.Please_Enter_Tell);
                if (input.tell.Length > 50)
                    throw BException.GenerateNewException(BMessages.Tell_CanNot_Be_MoreThen_50_Chars);
                if (string.IsNullOrEmpty(input.address))
                    throw BException.GenerateNewException(BMessages.Please_Enter_Address);
                if (input.address.Length > 1000)
                    throw BException.GenerateNewException(BMessages.Address_Length_Can_Not_Be_More_Then_4000);
                if (input.mapZoomRecivePlace.ToIntReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.Please_Select_Map);
                if (input.mapLatRecivePlace == null)
                    throw BException.GenerateNewException(BMessages.Please_Select_Map);
                if (input.mapLonRecivePlace == null)
                    throw BException.GenerateNewException(BMessages.Please_Select_Map);
                if (!new Point(input.mapLatRecivePlace.ToDoubleReturnNull().Value, input.mapLonRecivePlace.ToDoubleReturnNull().Value) { SRID = 4326 }.IsValid)
                    throw BException.GenerateNewException(BMessages.Please_Select_Map);
            }
            else
            {
                input.tell = null;
                input.address = null;
                input.mapZoomRecivePlace = null;
                input.mapLonRecivePlace = null;
                input.mapLatRecivePlace = null;
            }
            if (string.IsNullOrEmpty(input.reciveDate) || input.reciveDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            var startDateTime = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            var endDateTime = DateTime.Now.Date.AddDays(10).AddTicks(-1);
            if (startDateTime > input.reciveDate.ToEnDate().Value || input.reciveDate.ToEnDate().Value > endDateTime)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (
                    string.IsNullOrEmpty(input.reciveTime) ||
                    (input.reciveTime != "بازه 8 صبح تا 11" && input.reciveTime != "بازه 11 صبح تا 14" && input.reciveTime != "بازه 14 ظهر تا 17" && input.reciveTime != "بازه 17 عصر تا 20")
                )
                throw BException.GenerateNewException(BMessages.Please_Select_Time);


        }

        public object UploadImage(long? insuranceContractProposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> status)
        {
            var foundItemId = db.InsuranceContractProposalFilledForms
                .Where(t =>
                    t.Id == insuranceContractProposalFilledFormId && t.SiteSettingId == siteSettingId &&
                    t.CreateUserId == loginUserId && t.IsDelete != true && status.Contains(t.Status))
                .Select(t => t.Id)
                .FirstOrDefault();

            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.UploadNewFile(FileType.InsuranceContractProposalFilledForm, mainFile, loginUserId, siteSettingId, foundItemId, ".jpg,.png,.jpeg,.pdf,.doc,.docx", true);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
