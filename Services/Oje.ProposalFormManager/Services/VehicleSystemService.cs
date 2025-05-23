﻿using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class VehicleSystemService : IVehicleSystemService
    {
        readonly ProposalFormDBContext db = null;
        public VehicleSystemService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleSystems.Where(t => t.IsActive == true).OrderBy(t => t.Order).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? vehicleTypeId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSystems.OrderBy(t => t.Order).Where(t => t.IsActive == true && t.VehicleSystemVehicleTypes.Any(tt => tt.VehicleType.IsActive == true && tt.VehicleTypeId == vehicleTypeId));

            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public string GetTitleById(int? id)
        {
            return db.VehicleSystems.Where(t => t.Id == id).Select(t => t.Title).FirstOrDefault();
        }
    }
}
