using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class FireInsuranceTypeOfActivityManager: IFireInsuranceTypeOfActivityManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public FireInsuranceTypeOfActivityManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public List<FireInsuranceTypeOfActivity> GetBy(List<int> foundAllActivityIds)
        {
            return db.FireInsuranceTypeOfActivities
                .Where(t => t.IsActive == true && foundAllActivityIds.Contains(t.Id))
                .AsNoTracking()
                .ToList();
        }

        public object GetList(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_Your_Activity.GetEnumDisplayName() } };

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;


            var qureResult = db.FireInsuranceTypeOfActivities.Where(t => t.IsActive == true);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
