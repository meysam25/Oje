using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum TenderStatus
    {
        [Display(Name = "اسناد مناقصه تکمیل شده")]
        Create = 1,
        [Display(Name = "بازه اعتبار انتشار اسناد مناقصه")]
        FillDates = 2,
        [Display(Name = "محدوده انتشار اسناد برای مناقصه گر")]
        FillCondation = 3,
        [Display(Name = "منتشر شده")]
        Published = 4,
        [Display(Name = "اعلام قیمت مناقصه گر")]
        FillPrice = 5,
        [Display(Name = "انتخاب قیمت و تعیین مناقصه گر برنده")]
        SelectPrice = 6,
        [Display(Name = "صدور بیمه نامه توسط بیمه گر منتخب")]
        Issue = 7
    }
}
