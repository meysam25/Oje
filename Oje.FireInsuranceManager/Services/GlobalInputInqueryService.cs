using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Models.View;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class GlobalInputInqueryService: IGlobalInputInqueryService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public GlobalInputInqueryService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public long Create(FireInsuranceInquiryVM input, int? siteSettingId)
        {
            long result = 0;
            if (siteSettingId.ToIntReturnZiro() > 0 && input != null)
            {
                var allProbs = input.GetType().GetProperties();
                if (allProbs != null && allProbs.Count() > 0)
                {
                    GlobalInputInquery newInquery = new GlobalInputInquery() { SiteSettingId = siteSettingId.Value, CreateDate = DateTime.Now };

                    db.Entry(newInquery).State = EntityState.Added;
                    db.SaveChanges();


                    result = newInquery.Id;
                    foreach (var prop in allProbs)
                    {
                        GlobalInqueryInputParameter newInqueryParameter = new GlobalInqueryInputParameter()
                        {
                            GlobalInputInqueryId = result,
                            Key = prop.Name,
                            Value = prop.GetValue(input) + "",
                            Title = prop.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault()?.Name
                        };
                        if (string.IsNullOrEmpty(newInqueryParameter.Title))
                            newInqueryParameter.Title = prop.Name;
                        if (!string.IsNullOrEmpty(newInqueryParameter.Value) && !string.IsNullOrEmpty(newInqueryParameter.Key))
                        {
                            db.Entry(newInqueryParameter).State = EntityState.Added;
                        }
                    }
                    db.SaveChanges();
                }
            }

            return result;
        }
    }
}
