using Oje.AccountManager.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InsuranceContractManager: IInsuranceContractManager
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly IUserManager UserManager = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        public InsuranceContractManager(
                InquiryBaseDataDBContext db,
                IUserManager UserManager,
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager
            )
        {
            this.db = db;
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
        }

        public bool Exist(int id)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            return db.InsuranceContracts.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

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
