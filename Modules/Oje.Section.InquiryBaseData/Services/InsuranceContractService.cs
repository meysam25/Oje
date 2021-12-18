using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InsuranceContractService: IInsuranceContractService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        public InsuranceContractService(
                InquiryBaseDataDBContext db,
                IUserService UserService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        public bool Exist(int id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var childUserIds = UserService.GetChildsUserId(loginUserId.ToLongReturnZiro());

            return db.InsuranceContracts.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var childUserIds = UserService.GetChildsUserId(loginUserId.ToLongReturnZiro());

            result.AddRange(db.InsuranceContracts
                .Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)))
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
