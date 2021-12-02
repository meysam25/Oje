using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class CompanyManager : ICompanyManager
    {
        readonly ProposalFormDBContext db = null;
        public CompanyManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Company GetById(int? id)
        {
            return db.Companies.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Companies.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetLightListForInquiryDD()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            result.Add(new { id = "0", title = BMessages.I_Dont_Have_One.GetAttribute<DisplayAttribute>()?.Name });

            result.AddRange(db.Companies.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
