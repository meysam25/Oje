using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BlockClientConfigType
    {
        [Display(Name = "استعلام ثالث")]
        CarThirdInquiry = 1,
        [Display(Name = "استعلام بدنه")]
        CarBodyInquiry = 2,
        [Display(Name = "استعلام آتش سوزی")]
        FireInsuranceInquiry = 3,
        [Display(Name = "ایجاد فرم پیشنهاد")]
        CreateProposalFilledForm = 4,
        [Display(Name = "ثبت نظر برای بلاگ")]
        BlogReview = 5,
        [Display(Name = "پسندیدن بلاگ")]
        LikeBlog = 7,
        [Display(Name = "ثبت یادآوری فرم پیشنهاد")]
        CreateNewProposalFormReminder = 9,
        [Display(Name = "ورود با اس ام اس")]
        LoginWithSMS = 10,
        [Display(Name = "ورود با کلمه عبور")]
        LoginWithPassword = 11,
        [Display(Name = "بازیابی کلمه عبور با پیامک")]
        ActiveCodeForResetPassword = 12,
        [Display(Name = "برسی صحت کد پیامک شده برای بازیابی کلمه عبور")]
        CheckIfSmsCodeIsValid = 13,
        [Display(Name = "تغییر کلمه عبور بعد از برسی صحت کد پیامک شده")]
        ChangePasswordAndLogin = 14,
        [Display(Name = "عدم پیدا کردن صفحه")]
        PageNotFound = 15,
        [Display(Name = "تماس با ما")]
        ContactUs = 16,
        [Display(Name = "پیش ثبت نام کاربر")]
        CreateUserPreRegister = 17,
        [Display(Name = "استعلام ثبت خسارت")]
        ContractValidationCheck = 18,
        [Display(Name = "ثبت اعلام خسارت در وبسایت")]
        ContractCreate = 19,
        [Display(Name = "افزودن مدرک جدید برای خسارت")]
        WebsiteUserAddNewImageForInsuranceContract = 20
    }
}
