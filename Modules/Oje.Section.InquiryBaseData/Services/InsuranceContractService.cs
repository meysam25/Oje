using Microsoft.AspNetCore.Http;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.DB;
using Oje.Section.InquiryBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InsuranceContractService : IInsuranceContractService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractService(
                InquiryBaseDataDBContext db,
                IUserService UserService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public bool Exist(int id, int? siteSettingId)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            return db.InsuranceContracts.Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems).Any();
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            result.AddRange(db.InsuranceContracts
                .Where(t => t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title + " (" + t.ProposalForm.Title + ")"
                })
                .ToList());

            return result;
        }
    }
}
