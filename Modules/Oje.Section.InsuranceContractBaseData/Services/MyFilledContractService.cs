using Microsoft.AspNetCore.Http;
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
using System.Text;
using System.Threading.Tasks;

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

        public object GetList(MyFilledContractMainGrid searchInput, long? loginUserId, int? siteSettingId, List<InsuranceContractProposalFilledFormType> status)
        {
            searchInput = searchInput ?? new MyFilledContractMainGrid();

            var qureResult = db.InsuranceContractProposalFilledForms.Where(t => t.CreateUserId == loginUserId && t.SiteSettingId == siteSettingId && status.Contains(t.Status) && t.IsDelete != true);

            if (!string.IsNullOrEmpty(searchInput.contractTitle))
                qureResult = qureResult.Where(t => t.InsuranceContract.Title.Contains(searchInput.contractTitle));
            if (!string.IsNullOrEmpty(searchInput.familyMemebers))
                qureResult = qureResult.Where(t => t.InsuranceContractProposalFilledFormUsers.Any(tt => (tt.InsuranceContractUser.User.Firstname + " " + tt.InsuranceContractUser.User.Lastname).Contains(searchInput.familyMemebers)));
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
                    familyMemebers = t.InsuranceContractProposalFilledFormUsers.Select(tt => tt.InsuranceContractUser.User.Firstname + " " + tt.InsuranceContractUser.User.Lastname).ToList(),
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
