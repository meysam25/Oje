using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class GlobalInqueryService : IGlobalInqueryService
    {
        readonly ProposalFormDBContext db = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        public GlobalInqueryService(
                ProposalFormDBContext db,
                IPaymentMethodService PaymentMethodService
            )
        {
            this.db = db;
            this.PaymentMethodService = PaymentMethodService;
        }

        public void AppendInquiryData(long id, List<ProposalFilledFormPdfGroupVM> proposalFilledFormPdfGroupVMs)
        {
            if (id > 0)
            {
                var foundItem = db.GlobalInqueries
                    .Where(t => t.Id == id)
                    .Select(t => new
                    {
                        inquiryItems = t.GlobalInquiryItems.OrderBy(t => t.Order).Select(tt => new
                        {
                            title = tt.Title,
                            value = tt.Price
                        }).ToList(),
                        inputItems = t.GlobalInputInquery.GlobalInqueryInputParameters.Select(tt => new
                        {
                            title = tt.Title,
                            value = tt.Value,
                            key = tt.Key,
                            step = tt.ShowInDetailes
                        }).ToList()
                    }).FirstOrDefault();
                if (foundItem != null && foundItem.inquiryItems.Count > 0)
                {
                    if (proposalFilledFormPdfGroupVMs == null)
                        proposalFilledFormPdfGroupVMs = new();
                    ProposalFilledFormPdfGroupVM newGroupItem = new ProposalFilledFormPdfGroupVM() { title = "جزئیات استعلام", ProposalFilledFormPdfGroupItems = new() };
                    foreach (var item in foundItem.inquiryItems)
                        newGroupItem.ProposalFilledFormPdfGroupItems.Add(new ProposalFilledFormPdfGroupItem { title = item.title, value = item.value.ToString("###,###") + " ریال", cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });
                    proposalFilledFormPdfGroupVMs.Add(newGroupItem);
                }
                if (foundItem != null && foundItem.inputItems.Count > 0)
                {
                    if (proposalFilledFormPdfGroupVMs == null)
                        proposalFilledFormPdfGroupVMs = new();
                    ProposalFilledFormPdfGroupVM newGroupItem = new ProposalFilledFormPdfGroupVM() { title = "جزئیات محاسبه استعلام حق بیمه", ProposalFilledFormPdfGroupItems = new() };
                    foreach (var item in foundItem.inputItems)
                    {
                        if (!foundItem.inputItems.Any(t => !string.IsNullOrEmpty(t.key) && t.key != item.key && t.key.StartsWith(item.key)) && item.value.IndexOf("tem.Collections.Generic.Lis") == -1)
                            newGroupItem.ProposalFilledFormPdfGroupItems.Add(new ProposalFilledFormPdfGroupItem { title = item.title, value = item.value, cssClass = "col-md-3 col-sm-3 col-xs-12 col-lg-3" });

                    }
                    proposalFilledFormPdfGroupVMs.Add(newGroupItem);
                }
            }
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

        public GlobalInqueryResultVM GetInquiryDataList(long id, int proposalFormId)
        {
            if (id > 0)
            {
                return db.GlobalInqueries
                    .Where(t => t.Id == id && t.ProposalFormId == proposalFormId)
                    .Select(t => new GlobalInqueryResultVM
                    {
                        inquiryItems = t.GlobalInquiryItems.OrderBy(t => t.Order).Select(tt => new GlobalInqueryItemResultVM
                        {
                            title = tt.Title,
                            value = tt.Price.ToString("###,###") + "ریال"
                        }).ToList(),
                        inputItems = t.GlobalInputInquery.GlobalInqueryInputParameters.Select(tt => new GlobalInqueryItemResultVM
                        {
                            title = tt.Title,
                            value = tt.Value,
                            key = tt.Key,
                            step = tt.ShowInDetailes
                        }).ToList()
                    }).FirstOrDefault();
            }

            return null;
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

            if (!PaymentMethodService.Exist(siteSettingId, proposalFormId, foundItem.CompanyId))
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
