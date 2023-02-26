using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum TenderStatus
    {
        [Display(Name = "در انتظار آماده سازی اسناد")]
        W8FormPPDocuments = 1,
        [Display(Name = "در انتظار تایید اسناد")]
        W8ForConfirmDocuments = 2,
        [Display(Name = "در انتظار برگزاری مناقصه")]
        W8ForStartTeending = 3,
        [Display(Name = "تایید بازه زمانی انتشار")]
        ConfirmTenderDate = 4,
        [Display(Name = "مناقصات در حال برگزاری")]
        PublishedTender = 5,
        [Display(Name = "در حال اعلام قیمت")]
        BeenPriced = 6,
        [Display(Name = "در انتظار انتخاب برنده مناقصه")]
        W8ForSelectWinner = 7,
        [Display(Name = "انتخاب برنده مناقصه")]
        WinnerBeenSelected = 8,
        [Display(Name = "بیمه نامه صادر شده توسط مناقصه گر")]
        Issue = 9,

    }
}
