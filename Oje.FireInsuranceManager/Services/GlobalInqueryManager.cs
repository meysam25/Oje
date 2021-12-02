using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class GlobalInqueryManager: IGlobalInqueryManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public GlobalInqueryManager(FireInsuranceManagerDBContext db)
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
                    int order = 0;
                    foreach (var child in item.GlobalInquiryItems)
                    {
                        child.GlobalInquiryId = item.Id;
                        child.Order = ++order;
                        db.Entry(child).State = EntityState.Added;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
