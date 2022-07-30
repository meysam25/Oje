using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class GlobalInputInqueryService : IGlobalInputInqueryService
    {
        readonly ProposalFormDBContext db = null;
        public GlobalInputInqueryService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public long Create(object input, List<InqeryExteraParameter> inqeryExteraParameters, int? siteSettingId)
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
                            Title = prop.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault()?.Name,
                            ShowInDetailes = prop.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault()?.Description
                        };
                        if (string.IsNullOrEmpty(newInqueryParameter.Title))
                            newInqueryParameter.Title = prop.Name;
                        if (!string.IsNullOrEmpty(newInqueryParameter.Value) && !string.IsNullOrEmpty(newInqueryParameter.Key))
                            db.Entry(newInqueryParameter).State = EntityState.Added;
                    }
                    if (inqeryExteraParameters != null && inqeryExteraParameters.Count > 0)
                    {
                        foreach (var ePrameter in inqeryExteraParameters)
                        {
                            GlobalInqueryInputParameter newInqueryParameter = new GlobalInqueryInputParameter()
                            {
                                GlobalInputInqueryId = result,
                                Key = ePrameter.Title,
                                Value = ePrameter.Value,
                                Title = ePrameter.Title
                            };

                            if (!string.IsNullOrEmpty(newInqueryParameter.Value) && !string.IsNullOrEmpty(newInqueryParameter.Key))
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
