using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormPFService: ITenderFilledFormPFService
    {
        readonly TenderDBContext db = null;
        readonly IUserService UserService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TenderFilledFormPFService
            (
                TenderDBContext db,
                IUserService UserService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult AdminConfirm(long filledFormId, int jsonFormId)
        {
            var foundItem = db.TenderFilledFormPFs.Where(t => t.TenderFilledFormId == filledFormId && t.TenderProposalFormJsonConfigId == jsonFormId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsConfirmByAdmin = true;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public void Create(long tenderFilledFormId, int tenderProposalFormJsonConfigId)
        {
            db.Entry(new TenderFilledFormPF() 
            { 
                TenderFilledFormId = tenderFilledFormId, TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId 
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public void DactiveConfirmation(long filledFormId, int jsonFormId)
        {
            var foundItem = db.TenderFilledFormPFs.Where(t => t.TenderFilledFormId == filledFormId && t.TenderProposalFormJsonConfigId == jsonFormId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsConfirmByAdmin = false;
            foundItem.IsConfirmByUser = false;
            db.SaveChanges();
        }

        public object GetListForWeb(GlobalGridParentLong searchInput, long? tenderFilledFormId, long? loginUserId, int? siteSettingId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null)
        {
            searchInput = searchInput ?? new GlobalGridParentLong();
            (int? province, int? cityid, List<int> companyIds) = UserService.GetUserCityCompany(loginUserId);

            var quiryResult = 
                db.TenderFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .selectQuiryFilter(selectStatus, province, cityid, companyIds, loginUserId)
                .SelectMany(t => t.TenderFilledFormPFs)
                .Where(t => t.TenderFilledFormId == tenderFilledFormId);

            int row = searchInput.skip;

            return new 
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.TenderFilledFormId)
                .ThenByDescending(t => t.TenderProposalFormJsonConfigId)
                .Select(t => new 
                { 
                    fid = t.TenderFilledFormId,
                    cId = t.TenderProposalFormJsonConfigId,
                    insurance = t.TenderProposalFormJsonConfigId > 0 ? t.TenderProposalFormJsonConfig.ProposalForm.Title : "",
                    t.IsConfirmByAdmin,
                    t.IsConfirmByUser
                })
                .ToList()
                .Select(t => new 
                { 
                    row = ++row,
                    t.insurance,
                    t.fid,
                    t.cId,
                    id = t.fid + "_" + t.cId,
                    icU = t.IsConfirmByUser,
                    icA = t.IsConfirmByAdmin
                })
                .ToList()
            };
        }

        public ApiResult UserConfirm(long filledFormId, int jsonFormId)
        {
            var foundItem = db.TenderFilledFormPFs.Where(t => t.TenderFilledFormId == filledFormId && t.TenderProposalFormJsonConfigId == jsonFormId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (foundItem.IsConfirmByAdmin != true)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Active_By_Admin_First);

            foundItem.IsConfirmByUser = true;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
