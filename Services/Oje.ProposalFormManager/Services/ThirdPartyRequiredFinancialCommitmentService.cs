using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyRequiredFinancialCommitmentService : IThirdPartyRequiredFinancialCommitmentService
    {
        readonly ProposalFormDBContext db = null;
        readonly IInquiryCompanyLimitService InquiryCompanyLimitService = null;
        public ThirdPartyRequiredFinancialCommitmentService
            (
                ProposalFormDBContext db,
                IInquiryCompanyLimitService InquiryCompanyLimitService
            )
        {
            this.db = db;
            this.InquiryCompanyLimitService = InquiryCompanyLimitService;
        }

        public List<ThirdPartyRequiredFinancialCommitment> GetByIds(List<int> coverIds)
        {
            return db.ThirdPartyRequiredFinancialCommitments
                            .Where(t => t.IsActive == true)
                            .Include(t => t.ThirdPartyExteraFinancialCommitments).ThenInclude(t => t.ThirdPartyExteraFinancialCommitmentComs)
                            .Include(t => t.ThirdPartyRequiredFinancialCommitmentCompanies)
                            .AsNoTracking()
                            .Where(t => coverIds.Contains(t.Id))
                            .ToList();
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            var validComs = InquiryCompanyLimitService.GetCompanies(siteSettingId, Infrastructure.Enums.InquiryCompanyLimitType.ThirdParty);
            var qureResult = db.ThirdPartyRequiredFinancialCommitments.Where(t => t.IsActive == true && t.ThirdPartyExteraFinancialCommitments.Any(tt => tt.IsActive == true));

            if (validComs != null && validComs.Count > 0)
            {
                var allValidComIds = validComs.Select(t => t.Id).ToList();
                qureResult = qureResult.Where(t => t.ThirdPartyRequiredFinancialCommitmentCompanies.Any(tt => allValidComIds.Contains(tt.CompanyId)));
            }

            result.AddRange(qureResult.OrderBy(t => t.Order).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetLightListShortTitle(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            var validComs = InquiryCompanyLimitService.GetCompanies(siteSettingId, Infrastructure.Enums.InquiryCompanyLimitType.ThirdParty);
            var qureResult = db.ThirdPartyRequiredFinancialCommitments.Where(t => t.IsActive == true && t.ThirdPartyExteraFinancialCommitments.Any(tt => tt.IsActive == true));

            if (validComs != null && validComs.Count > 0)
            {
                var allValidComIds = validComs.Select(t => t.Id).ToList();
                qureResult = qureResult.Where(t => t.ThirdPartyRequiredFinancialCommitmentCompanies.Any(tt => allValidComIds.Contains(tt.CompanyId)));
            }

            result.AddRange(qureResult.OrderBy(t => t.Order).Select(t => new { id = t.Id, title = t.ShortTitle }).ToList());

            return result;
        }
    }
}
