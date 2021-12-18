using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class SiteSettingService: ISiteSettingService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        public SiteSettingService(ProposalFormBaseDataDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
