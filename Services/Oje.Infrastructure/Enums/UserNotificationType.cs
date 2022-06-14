using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum UserNotificationType
    {
        [Display(Name = "ثبت فرم پیشنهاد جدید")]
        NewProposalFilledForm = 1,
        [Display(Name = "حذف فرم پیشنهاد")]
        ProposalFilledFormDeleted = 2,
        [Display(Name = "ویرایش فرم پیشنهاد")]
        ProposalFilledFormEdited = 3,
        [Display(Name = "ارجاع فرم پیشنهاد")]
        ReferToUser = 4,
        [Display(Name = "تغییر شرکت فرم پیشنهاد")]
        ProposalFilledFormCompanyChanged = 5,
        [Display(Name = "تغییر نماینده فرم پیشنهاد")]
        ProposalFilledFormAgentChanged = 6,
        [Display(Name = "انتخاب مبلغ برای فرم پیشنهاد")]
        ProposalFilledFormPriceSelected = 7,
        [Display(Name = "ثبت مدرک جدید برای فرم پیشنهاد")]
        ProposalFilledFormNewDocument = 8,
        [Display(Name = "حذف مدرک برای فرم پیشنهاد")]
        ProposalFilledFormDocumentDeleted = 9,
        [Display(Name = "افزود مدرک مالی جدید برای فرم پیشنهاد")]
        ProposalFilledFormNewFinancialDocuemnt = 10,
        [Display(Name = "حذف مدرک مالی برای فرم پیشنهاد")]
        ProposalFilledFormFinancialDocuemntDeleted = 11,
        [Display(Name = "ویرایش مدرک مالی برای فرم پیشنهاد")]
        ProposalFilledFormFinancialDocumentEdited = 12,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به جدید")]
        ProposalFilledFormStatusChangedNew = 13,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به در انتظار تایید")]
        ProposalFilledFormStatusChangeW8ForConfirm = 14,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به نیاز به کارشناس")]
        ProposalFilledFormStatusChangeNeedSpecialist = 15,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به تایید شده")]
        ProposalFilledFormStatusChangeConfirm = 16,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به صادره")]
        ProposalFilledFormStatusChangeIssue = 17,
        [Display(Name = "تغییر وضعیت فرم پیشنهاد به رد شده")]
        ProposalFilledFormStatusChangeNotIssue = 18,
        [Display(Name = "ورود")]
        Login = 19,
        [Display(Name = "ثبت نام")]
        Register = 20,
        [Display(Name = "ثبت نام موفقیت آمیز")]
        RegisterSuccessFull = 21,
        [Display(Name = "فراموشی کلمه عبور")]
        ForgetPassword = 22,
        [Display(Name = "ثبت اعلام خسارت جدید")]
        CreateNewDamageClime = 23,
        [Display(Name = "تغییر وضعیت اعلام خسارت")]
        DamageClimeStatusChange = 24,
        [Display(Name = "تغییر قیمت اعلام خسارت")]
        DamageClimeSetPrice = 25,
        [Display(Name = "تیکت جدید")]
        NewTicket = 26,
        [Display(Name = "پیام جدید برای ادمین در چت انلاین")]
        NewOnlineMessageForAdmin = 27,
        [Display(Name = "افزودن بیمه شده")]
        NewInsuranceContractUser = 28,
        [Display(Name = "کاربر پروفایل خود را به روز رسانی کرد")]
        UserUpdateHisProfile = 29,
        [Display(Name = "به روز رسانی قیمت مناقصه")]
        UpdateTenderDates = 30,
        [Display(Name = "مشخص کردن شهر و استان")]
        UpdateTenderAccess = 31,
        [Display(Name = "انتشار مناقصه")]
        PublishTender = 32,
        [Display(Name = "انتخاب نماینده مناقصه")]
        TenderSelectPrice = 33,
        [Display(Name = "افزودن قیمت برای مناقصه")]
        AddTenderPrice = 34,
        [Display(Name = "به روز رسانی قیمت برای مناقصه")]
        UpdateTenderPrice = 35,
        [Display(Name = "انتشار مبلغ مناقصه توسط نماینده")]
        PublishTenderPrice = 36,
        [Display(Name = "حذف قیمت تایین شده توسط نماینده")]
        DeleteTenderPrice = 37,
        [Display(Name = "صدور بیمه نامه")]
        IssueTender = 38,
        [Display(Name = "به روز رسانی صدور بیمه نامه")]
        UpdateIssueTender = 39,
        [Display(Name = "افزودن پیام جدید")]
        AddNewMessage = 40
    }
}
