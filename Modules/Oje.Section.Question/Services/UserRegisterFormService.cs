using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.Question.Interfaces;
using Oje.Section.Question.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Question.Services
{
    public class UserRegisterFormService : IUserRegisterFormService
    {
        readonly QuestionDBContext db = null;
        public UserRegisterFormService(QuestionDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.UserRegisterForms.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.UserRegisterForms.Where(t => t.SiteSettingId == siteSettingId).Select(t => new 
            {
                id = t.Id,
                title = t.Title,
            }).ToList());

            return result;
        }
    }
}
