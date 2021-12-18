using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class BankService : IBankService
    {
        readonly ProposalFormDBContext db = null;
        public BankService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };
            result.AddRange(db.Banks.Where(t => t.IsActive == true).Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }

        public bool IsValid(List<int> bankIds)
        {
            return bankIds.Count == db.Banks.Where(t => bankIds.Contains(t.Id) && t.IsActive == true).Count();
        }
    }
}
