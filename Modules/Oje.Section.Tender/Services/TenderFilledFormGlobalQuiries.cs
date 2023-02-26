using Oje.Infrastructure.Enums;
using Oje.Section.Tender.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public static class TenderFilledFormGlobalQuiries
    {
        public static IQueryable<TenderFilledForm> selectQuiryFilter(
                this IQueryable<TenderFilledForm> input, TenderSelectStatus? selectStatus,
                int? province, int? cityid, List<int> companyIds, long? loginUserId
            )
        {
            if (companyIds == null)
                companyIds = new List<int>();
            if (selectStatus != null)
            {
                //supper admin
                if (selectStatus == TenderSelectStatus.Current)
                    return input.Where(t => !t.TenderFilledFormIssues.Any() && t.AvalibleDate >= DateTime.Now && !t.TenderFilledFormPrices.Any());
                else if (selectStatus == TenderSelectStatus.Expired)
                    return input.Where(t => !t.TenderFilledFormIssues.Any() && t.AvalibleDate < DateTime.Now);
                else if (selectStatus == TenderSelectStatus.BeenPriced)
                    return input.Where(t => t.TenderFilledFormPrices.Any() && !t.TenderFilledFormIssues.Any());
                else if (selectStatus == TenderSelectStatus.Issued)
                    return input.Where(t => t.TenderFilledFormIssues.Any());
                else if (selectStatus == TenderSelectStatus.Consultation)
                    return input.Where(t => t.IsPublished != true);
                else
                {
                    //tender
                    if (selectStatus == TenderSelectStatus.CurrentTender)
                        return input.Where(t =>
                                                (t.CityId == null || cityid == null || t.CityId == cityid) &&
                                                (t.ProvinceId == null || province == null || t.ProvinceId == province) &&
                                                (!t.TenderFilledFormValidCompanies.Any() || companyIds == null || companyIds.Count == 0 || t.TenderFilledFormValidCompanies.Any(tt => companyIds.Contains(tt.CompanyId)))
                                        )
                                    .Where(t =>
                                        (t.OpenDate != null && t.OpenDate <= DateTime.Now && t.AvalibleDate != null && t.AvalibleDate >= DateTime.Now)
                                    )
                                    .Where(t => !t.TenderFilledFormIssues.Any())
                                    .Where(t => 
                                        t.TenderFilledFormPrices.Count(tt => tt.UserId == loginUserId && tt.IsPublished == true) != t.TenderFilledFormPFs.Count()
                                    )
                       ;
                    else if (selectStatus == TenderSelectStatus.BeenPricedTender)
                        return input.Where(t =>
                                                (t.CityId == null || cityid == null || t.CityId == cityid) &&
                                                (t.ProvinceId == null || province == null || t.ProvinceId == province) &&
                                                (!t.TenderFilledFormValidCompanies.Any() || t.TenderFilledFormValidCompanies.Any(tt => companyIds.Contains(tt.CompanyId)))
                                        )
                                    .Where(t =>
                                        (t.OpenDate != null && t.OpenDate <= DateTime.Now && t.AvalibleDate != null && t.AvalibleDate >= DateTime.Now)
                                    )
                                    .Where(t => t.TenderFilledFormIssues.Count() != t.TenderFilledFormPFs.Count())
                                    .Where(t =>
                                        t.TenderFilledFormPrices.Count(tt => tt.UserId == loginUserId && tt.IsConfirm == true) == t.TenderFilledFormPFs.Count()
                                    )
                       ;
                    else if (selectStatus == TenderSelectStatus.IssuedTender)
                        return input.Where(t =>
                                                (t.CityId == null || cityid == null || t.CityId == cityid) &&
                                                (t.ProvinceId == null || province == null || t.ProvinceId == province) &&
                                                (!t.TenderFilledFormValidCompanies.Any() || t.TenderFilledFormValidCompanies.Any(tt => companyIds.Contains(tt.CompanyId)))
                                        )
                                    .Where(t => t.TenderFilledFormIssues.Count(t => t.UserId == loginUserId) == t.TenderFilledFormPFs.Count())
                       ;
                    else
                    {

                        // tenderUser
                        if (selectStatus == TenderSelectStatus.NewTenderUser)
                            return input.Where(t => t.UserId == loginUserId && t.TenderFilledFormPFs.Count() != t.TenderFilledFormPFs.Count(tt => tt.IsConfirmByAdmin == true && tt.IsConfirmByUser == true));
                        else if (selectStatus == TenderSelectStatus.CurrentTenderUser)
                            return input.Where(t => t.UserId == loginUserId && t.TenderFilledFormPFs.Count() == t.TenderFilledFormPFs.Count(tt => tt.IsConfirmByAdmin == true && tt.IsConfirmByUser == true) && !t.TenderFilledFormPrices.Any(t => t.IsPublished == true) && !t.TenderFilledFormPrices.Any());
                        else if (selectStatus == TenderSelectStatus.BeenPriceTenderUser)
                            return input.Where(t => t.UserId == loginUserId && t.TenderFilledFormPrices.Any() && t.TenderFilledFormIssues.Count() != t.TenderFilledFormPFs.Count() && t.IsPublished == true);
                        else if (selectStatus == TenderSelectStatus.IssueTenderUser)
                            return input.Where(t => t.UserId == loginUserId && t.TenderFilledFormIssues.Count() == t.TenderFilledFormPFs.Count() && t.IsPublished == true);
                    }
                }
            }

            return input
                .Where(t =>
                        (t.CityId == null || cityid == null || t.CityId == cityid) &&
                        (t.ProvinceId == null || province == null || t.ProvinceId == province) &&
                        (!t.TenderFilledFormValidCompanies.Any() || t.TenderFilledFormValidCompanies.Any(tt => companyIds.Contains(tt.CompanyId)))
                    )
                .Where(t => (loginUserId == null || t.UserId == loginUserId))
                .Where(t =>
                        (t.OpenDate != null && t.OpenDate <= DateTime.Now && t.AvalibleDate != null && t.AvalibleDate >= DateTime.Now) ||
                        t.TenderFilledFormPrices.Any(t => t.UserId == loginUserId)
                        )
            ;

        }
    }
}
