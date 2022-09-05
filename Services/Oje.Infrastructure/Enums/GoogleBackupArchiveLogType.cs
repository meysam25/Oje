using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum GoogleBackupArchiveLogType: byte
    {
        [Display(Name = "اپلود")]
        UploadSection = 1,
        [Display(Name = "ساخت فایل زیپ فایل های بارگزاری شده")]
        GenerateImageZip = 2,
        [Display(Name = "ساخت بک اپ پایگاه داده")]
        SqlGenerateBackup = 3,
        [Display(Name = "حذف فایل های اکسپایر شده")]
        RemoveExpiredFile = 4,
        [Display(Name = "بخش نا مشخص")]
        UnknownSection = 99
    }
}
