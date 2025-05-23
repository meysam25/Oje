﻿using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        readonly ProposalFormDBContext db = null;
        public VehicleTypeService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public VehicleType GetById(int? id)
        {
            return db.VehicleTypes.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleTypes.OrderBy(t => t.Order).Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetSpacCatTitleBy(int? vehicleTypeId)
        {
            return new { title = db.VehicleTypes.OrderBy(t => t.Order).Where(t => t.IsActive == true && t.Id == vehicleTypeId).Select(t => t.VehicleSpecCategory.Title).FirstOrDefault() };
        }
    }
}
