using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class CityService : ICityService
    {
        readonly AccountDBContext db = null;
        public CityService(AccountDBContext db)
        {
            this.db = db;
        }

        public int? Create(int? provinceId, string title, bool isActive)
        {
            var newItem = new City()
            {
                IsActive = isActive,
                ProvinceId = provinceId.Value,
                Title = title
            };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem.Id;
        }

        public bool Exist(int provinceId, int id)
        {
            return db.Cities.Any(t => t.ProvinceId == provinceId && t.Id == id && t.IsActive == true);
        }

        public int? GetIdBy(string title)
        {
            return db.Cities.Where(t => t.Title == title).Select(t => t.Id).FirstOrDefault();
        }

        public object GetLightList(int? provinceId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Cities.Where(t => t.ProvinceId == provinceId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetSelect2List(int? provinceId, Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Cities.OrderByDescending(t => t.Id).Where(t => t.ProvinceId == provinceId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Title).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
