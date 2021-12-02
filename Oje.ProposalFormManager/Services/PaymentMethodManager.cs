using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class PaymentMethodManager : IPaymentMethodManager
    {
        readonly ProposalFormDBContext db = null;
        public PaymentMethodManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public bool Exist(int? siteSettingId, int proposalFormId, int companyId)
        {
            return db.PaymentMethods.Any(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.IsActive == true && t.PaymentMethodCompanies.Any(tt => tt.CompanyId == companyId));
        }

        public PaymentMethodDetailesVM GetItemDetailes(int paymentMethodId, int? siteSettingId, long inquiryPrice, int proposalFormId)
        {
            var emptyResult = new PaymentMethodDetailesVM { prePayment = "0" };
            if (inquiryPrice <= 0)
                return emptyResult;
            var paymentMethod = db.PaymentMethods.Include(t => t.PaymentMethodFiles).Where(t => t.Id == paymentMethodId && t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).FirstOrDefault();
            if (paymentMethod == null)
                return emptyResult;

            var parePaymentPrice = paymentMethod.PrePayPercent != null ? ((inquiryPrice / (decimal)100) * paymentMethod.PrePayPercent).ToLongReturnZiro() : (long)0;
            var remaindPrice = inquiryPrice - parePaymentPrice;
            List<PaymentMethodDetailesCheckVM> checkArr = new List<PaymentMethodDetailesCheckVM>();

            if (paymentMethod.DebitCount != null && paymentMethod.DebitCount.Value > 0)
            {
                var eachPayment = Math.Ceiling((decimal)remaindPrice / (decimal)paymentMethod.DebitCount.Value).ToLongReturnZiro();
                var curDateTime = DateTime.Now;
                for (var i = 0; i < paymentMethod.DebitCount.Value; i++)
                {
                    curDateTime = curDateTime.AddMonths(1);
                    checkArr.Add(new PaymentMethodDetailesCheckVM { checkDate = curDateTime.ToFaDate(), checkDateEn = curDateTime, eachPayment = eachPayment.ToString("###,###"), eachPaymentLong = eachPayment });
                }
            }
            List<PaymentMethodDetailesFilesVM> paymentRequreFiles = new List<PaymentMethodDetailesFilesVM>();

            if (paymentMethod.PaymentMethodFiles != null && paymentMethod.PaymentMethodFiles.Count > 0)
            {
                foreach (var file in paymentMethod.PaymentMethodFiles)
                    paymentRequreFiles.Add(new PaymentMethodDetailesFilesVM 
                    {
                        isRequired = file.IsRequired,
                        title = file.Title,
                        sample = GlobalConfig.FileAccessHandlerUrl + file.DownloadImageUrl
                    });
            }

            return new PaymentMethodDetailesVM(checkArr, paymentRequreFiles)
            {
                prePayment = parePaymentPrice > 0 ? parePaymentPrice.ToString("###,###") : "0",
                prePaymentLong = parePaymentPrice,
                debitCount = paymentMethod.DebitCount != null ? paymentMethod.DebitCount.Value : 0
            };
        }

        public List<IdTitle> GetLightList(int proposalFormId, int? siteSettingId, int companyId)
        {
            return db.PaymentMethods
                .OrderBy(t => t.IsDefault == true ? -99 : t.Order)
                .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.PaymentMethodCompanies.Any(tt => tt.CompanyId == companyId))
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                })
                .ToList()
                .Select(t => new IdTitle
                {
                    id = t.id.ToString(),
                    title = t.title
                })
                .ToList()
                ;
        }

        public List<PaymentMethod> GetList(int? siteSettingId, int? proposalFormId)
        {
            return db.PaymentMethods.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.IsActive == true).AsNoTracking().ToList();
        }

        public bool IsValid(int id, int? siteSettingId, int proposalFormId, int companyId)
        {
            return db.PaymentMethods
                .Any(t =>
                        t.SiteSettingId == siteSettingId && t.Id == id &&
                        t.ProposalFormId == proposalFormId && t.IsActive == true &&
                        t.PaymentMethodCompanies.Any(tt => tt.CompanyId == companyId)
                    );
        }
    }
}
