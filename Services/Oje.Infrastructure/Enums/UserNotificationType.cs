using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ProposalFilledFormDocumentDeleted = 9 ,
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
        NewOnlineMessageForAdmin = 27
    }
}
