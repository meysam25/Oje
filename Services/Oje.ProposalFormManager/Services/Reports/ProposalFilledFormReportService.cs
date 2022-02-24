using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces.Reports;
using Oje.ProposalFormService.Models.DB.Reports;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services.Reports
{
    public class ProposalFilledFormReportService : IProposalFilledFormReportService
    {
        readonly ProposalFormReportDBContext db = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        public ProposalFilledFormReportService(
                ProposalFormReportDBContext db,
                AccountService.Interfaces.IUserService UserService
            )
        {
            this.db = db;
            this.UserService = UserService;
        }

        IQueryable<ProposalFilledForm> getBaseQueiry(int? siteSettingId, long? userId)
        {
            var allChildUserId = UserService.CanSeeAllItems(userId.ToLongReturnZiro());
            var resultObjQoury = db.ProposalFilledForms.Where(t => t.SiteSettingId == siteSettingId && t.IsDelete != true);
            resultObjQoury = resultObjQoury
                .Where(t =>
                    (
                    allChildUserId == true ||
                    t.ProposalFilledFormUsers.Any
                     (tt => tt.UserId == userId ||
                        tt.User.Parent.Id == userId ||
                        tt.User.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId ||
                        tt.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == userId
                     )
                  )
                );
            return resultObjQoury;
        }

        public object GetPaymentChartReport(int? siteSettingId, long? userId)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId);

            resultData.Add(new { name = "پرداخت شده", y = resultObjQoury.Where(t => !string.IsNullOrEmpty(t.PaymentTraceCode)).Count() });
            resultData.Add(new { name = "پرداخت نشده", y = resultObjQoury.Where(t => string.IsNullOrEmpty(t.PaymentTraceCode)).Count() });

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }

        public object GetStatusChartReport(int? siteSettingId, long? userId)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId);

            var grouptedItems = resultObjQoury.GroupBy(t => t.Status).Select(t => new { status = t.Key, count = t.Count() }).ToList();

            foreach (var item in grouptedItems)
            {
                resultData.Add(new { name = item.status.GetEnumDisplayName(), y = item.count });
            }

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }

        public object GetProposalFormChartReport(int? siteSettingId, long? userId)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId);

            var grouptedItems = resultObjQoury.GroupBy(t => t.ProposalFormId).Select(t => new { ppfTitle = t.FirstOrDefault().ProposalForm.Title, count = t.Count() }).ToList();

            foreach (var item in grouptedItems)
            {
                resultData.Add(new { name = item.ppfTitle, y = item.count });
            }

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }

        public object GetProposalFormAgentsChartReport(int? siteSettingId, long? userId)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId)
                .SelectMany(t => t.ProposalFilledFormUsers)
                .Where(t => t.Type == Infrastructure.Enums.ProposalFilledFormUserType.Agent)
                .Select(t => t.User)
                .Distinct()
                .Select(t => new
                {
                    fullName = t.Firstname + " " + t.Lastname,
                    countPPF = t.ProposalFilledFormUsers.Count
                }).ToList();


            foreach (var item in resultObjQoury)
            {
                resultData.Add(new { name = item.fullName, y = item.countPPF });
            }

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }

        public object GetProposalFormCompanyChartReport(int? siteSettingId, long? userId)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId)
                .SelectMany(t => t.ProposalFilledFormCompanies)
                .Where(t => t.IsSelected == true)
                .Select(t => t.Company)
                .Distinct()
                .Select(t => new
                {
                    title = t.Title,
                    countPPF = t.ProposalFilledFormCompanies.Where(tt => tt.IsSelected == true).Count()
                }).ToList();


            foreach (var item in resultObjQoury)
            {
                resultData.Add(new { name = item.title, y = item.countPPF });
            }

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }

        public object GetProposalFormTimeChartReport(int? siteSettingId, long? userId, DateTime beginTime)
        {
            List<object> resultSeries = new List<object>();
            List<object> resultData = new List<object>();

            var resultObjQoury = getBaseQueiry(siteSettingId, userId);
            resultObjQoury = resultObjQoury.Where(t => t.CreateDate >= beginTime);

            var grouptedItems = resultObjQoury.GroupBy(t => t.ProposalFormId).Select(t => new { ppfTitle = t.FirstOrDefault().ProposalForm.Title, count = t.Count() }).ToList();

            foreach (var item in grouptedItems)
            {
                resultData.Add(new { name = item.ppfTitle, y = item.count });
            }

            resultSeries.Add(new { name = "فرم پیشنهاد", colorByPoint = true, data = resultData });

            return resultSeries;
        }
    }
}
