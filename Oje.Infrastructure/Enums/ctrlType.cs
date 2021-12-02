using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        dropDown2 = 6
    }
}
