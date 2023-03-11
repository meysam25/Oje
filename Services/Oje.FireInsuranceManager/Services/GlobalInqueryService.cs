using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;

namespace Oje.FireInsuranceService.Services
{
    public class GlobalInqueryService: IGlobalInqueryService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public GlobalInqueryService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public void Create(List<GlobalInquery> result)
        {
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.CreateDate = DateTime.Now;
                    db.Entry(item).State = EntityState.Added;
                }

                db.SaveChanges();

                foreach (var item in result)
                {
                    item.FilledSignature();
                }

                foreach (var item in result)
                {
                    int order = 0;
                    foreach (var child in item.GlobalInquiryItems)
                    {
                        child.GlobalInquiryId = item.Id;
                        child.Order = ++order;
                        child.FilledSignature();
                        db.Entry(child).State = EntityState.Added;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
