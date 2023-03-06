using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum ctrlType
    {
        [Display(Name = "دراپ دان")]
        dropDown = 1,
        [Display(Name = "تکس")]
        text = 2,
        [Display(Name = "بارگزاری فایل چنتایی")]
        dynamicFileUpload = 3,
        [Display(Name ="اقساط کنترول چک")]
        chkCtrlBox = 4,
        [Display(Name = "بارگزاری فایل اقساط")]
        dynamicFileUploadDepend = 5,
        [Display(Name ="دراپ دان جستجو")]
        dropDown2 = 6,
        [Display(Name = "چک باکس")]
        checkBox = 7,
        [Display(Name = "توکن باکس 1")]
        tokenBox = 8,
        [Display(Name = "خالی")]
        empty = 9,
        [Display(Name = "تاریخ فارسی")]
        persianDateTime = 10,
        [Display(Name = "ردیو باکس")]
        radio = 11,
        [Display(Name = "چند انتخابی")]
        multiRowInput = 12,
        [Display(Name = "سرور تمپلیت")]
        cTemplate = 13,
        [Display(Name = "دکمه")]
        button = 14,
        [Display(Name = "نقشه")]
        map = 15,
        [Display(Name = "متن")]
        label = 16,
        [Display(Name = "پلاک")]
        carPlaque = 17,
        [Display(Name = "تکس اریا")]
        textarea = 18,
        [Display(Name = "عدد")]
        number = 19,
        [Display(Name = "تمپلیت")]
        template = 20

    }
}
