using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum TenderSelectStatus: byte
    {
        [Display(Name = "جاری")]
        Current = 1,
        [Display(Name = "مناقضی شده")]
        Expired = 2,
        [Display(Name = "اعلام نرخ شده")]
        BeenPriced = 3,
        [Display(Name = "تایید شده")]
        Issued = 4,
        [Display(Name = "مشاوره")]
        Consultation = 5,



        [Display(Name = "جاری مناقصه گر")]
        CurrentTender = 6,
        [Display(Name = "اعلام نرخ شده مناقصه گر")]
        BeenPricedTender = 7,
        [Display(Name = "برنده شده")]
        IssuedTender = 8,



        [Display(Name = "مناقصات جدید مناقصه گذار")]
        NewTenderUser = 9,
        [Display(Name = "اعلام قیمت شده")]
        BeenPriceTenderUser = 10,
        [Display(Name = "صادره")]
        IssueTenderUser = 11,
        [Display(Name = "مناقصات جاری")]
        CurrentTenderUser = 12
    }
}
