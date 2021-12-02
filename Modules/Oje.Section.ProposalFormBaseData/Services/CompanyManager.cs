using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class CompanyManager: ICompanyManager
    {
        readonly ProposalFormBaseDataDBContext db = null;
        public CompanyManager(ProposalFormBaseDataDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Companies.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
