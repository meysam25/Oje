using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class UserRegisterFormService : IUserRegisterFormService
    {
        readonly TenderDBContext db = null;
        public UserRegisterFormService
            (
                TenderDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int id, int? siteSettingId)
        {
            return db.UserRegisterForms.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                (
                    db.UserRegisterForms
                        .Where(t => t.SiteSettingId == siteSettingId)
                        .Select(t => new { id = t.Id, title = t.Title })
                        .ToList()
                );

            return result;
        }
    }
}
