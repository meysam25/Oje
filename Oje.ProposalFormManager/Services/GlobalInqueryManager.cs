using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class GlobalInqueryManager : IGlobalInqueryManager
    {
        readonly ProposalFormDBContext db = null;
        readonly IPaymentMethodManager PaymentMethodManager = null;
        public GlobalInqueryManager(
                ProposalFormDBContext db,
                IPaymentMethodManager PaymentMethodManager
            )
        {
            this.db = db;
            this.PaymentMethodManager = PaymentMethodManager;
        }
        public void Create(List<GlobalInquery> inputs)
        {
            if (inputs.Count > 0)
            {
                foreach (var item in inputs)
                {
                    item.CreateDate = DateTime.Now;
                    db.Entry(item).State = EntityState.Added;
                }

                db.SaveChanges();

                foreach (var item in inputs)
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

        public int GetCompanyId(long id, int? siteSettingId)
        {
            return db.GlobalInqueries.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.CompanyId).FirstOrDefault();
        }

        public object GetSumPrice(long inquiryId, int proposalFormId, int? siteSettingId)
        {
            var foundItem = db.GlobalInqueries.Where(t => t.Id == inquiryId && t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId).Include(t => t.GlobalInquiryItems).FirstOrDefault();

            if (foundItem == null)
                return false;

            if ((DateTime.Now - foundItem.CreateDate).TotalMinutes > 60)
                return false;

            if (foundItem.GlobalInquiryItems.Any(t => t.CalcKey == "BCash"))
                return false;

            if (!PaymentMethodManager.Exist(siteSettingId, proposalFormId, foundItem.CompanyId))
                return false;

            return new { wolePrice = foundItem.GlobalInquiryItems.Where(t => t.Expired != true).Sum(t => t.Price).ToString("###,###") };
        }

        public long GetSumPriceLong(long id, int proposalFormId, int? siteSettingId)
        {
            return db.GlobalInqueries
                .Where(t => t.Id == id && t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId)
                .SelectMany(t => t.GlobalInquiryItems)
                .Where(t => t.Expired != true)
                .Sum(t => t.Price);
        }

        public bool HasAnyCashDiscount(long inQuiryId)
        {
            return db.GlobalInqueries.Any(t => t.Id == inQuiryId && t.GlobalInquiryItems.Any(tt => tt.CalcKey == "BCash"));
        }

        public bool IsValid(long id, int? siteSettingId, int proposalFormId)
        {
            var knowDT = DateTime.Now.AddHours(-1);
            return db.GlobalInqueries
                .Any
                (
                    t => t.Id == id && t.SiteSettingId == siteSettingId && t.CreateDate >= knowDT && t.ProposalFormId == proposalFormId
                );
        }
    }
}
