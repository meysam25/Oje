using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum FileType
    {
        [Display(Name = "تصویر کاربر", Prompt = "UserProfilePic", Description = "300*300")]
        UserProfilePic = 1,
        [Display(Name = "تصویر شرکت بیمه", Prompt = "CompanyLogo", Description = "500*500")]
        CompanyLogo = 2,
        [Display(Name = "قوانین فرم پیشنهاد", Prompt = "ProposalFormRules")]
        ProposalFormRules = 3,
        [Display(Name = "مدارک مورد نیاز", Prompt = "RequiredDocument", Description = "1000*600")]
        RequiredDocument = 4,
        [Display(Name = "تصاویر سی کا ادیتور", Prompt = "CKEditor", Description = "~1000*1000")]
        CKEditor = 5,
        [Display(Name = "تصاویر تعهدات قرارداد", Prompt = "InsuranceContractCommitment")]
        InsuranceContractCommitment = 6,
        [Display(Name = "مدارک بیمه شدگان", Prompt = "ContractUserDocuemnt", Description = "500*500")]
        ContractUserDocuemnt = 7,
        [Display(Name = "مدارک مورد نیاز جهت فروش اقساطی", Prompt = "DebitSellRequredDocument", Description = "~1000*1000")]
        DebitSellRequredDocument = 8,
        [Display(Name = "تصویر بانک", Prompt = "BankPick", Description = "300*300")]
        BankLogo = 9,
        [Display(Name = "مدارک فرم پیشنهاد", Prompt = "ProposalFilledForm", Description = "~1000*1000", AutoGenerateFilter = true)]
        ProposalFilledForm = 10,
        [Display(Name ="مدارک تعیین قیمت شرکت", Prompt = "ProposalFilledFormCompanies")]
        CompanyPrice = 11,
        [Display(Name = "تصویر بزرگ بلاگ", Prompt = "Blog", Description = "1000*800")]
        BlogBigImage = 12,
        [Display(Name = "تصویر متوسط بلاگ", Prompt = "Blog", Description = "200*160")]
        BlogMedImage = 13,
        [Display(Name = "تصویر کوچک بلاگ", Prompt = "Blog", Description = "80*80")]
        BlogSmallImage = 14,
        [Display(Name = "فایل صوتی بلاگ", Prompt = "BlogSound")]
        BlogSound = 15,
        [Display(Name = "تصویر بزرگ بلاگ 600", Prompt = "Blog", Description = "600*400")]
        BlogBigImage600 = 16,
        [Display(Name = "تصویر اصلی صفحه", Prompt = "Page", Description = "1900*380", AutoGenerateField = false)]
        PageMainImage = 17,
        [Display(Name = "تصویر اصلی صفحه (کوچک)", Prompt = "Page", Description = "800*600", AutoGenerateField = false)]
        PageMainImageSmall = 18,
        [Display(Name = "تصویر دیزاین چپ و راست", Prompt = "PageLeftRightDesignItem", Description = "560*373")]
        PageLeftRightDesignItem = 19,
        [Display(Name = "خصوصیات", Prompt = "Property")]
        Property = 20,
        [Display(Name = "لوگوی سایت 96", Prompt = "MainLogo", Description = "96*96", AutoGenerateFilter = true)]
        MainLogo96 = 21,
        [Display(Name = "لوگوی سایت 192", Prompt = "MainLogo", Description = "192*192", AutoGenerateFilter = true)]
        MainLogo192 = 22,
        [Display(Name = "لوگوی سایت 512", Prompt = "MainLogo", Description = "512*512", AutoGenerateFilter = true)]
        MainLogo512 = 23,
        [Display(Name = "تصویر شرکت بیمه (کوچک)", Prompt = "CompanyLogo", Description = "32*32")]
        CompanyLogo32 = 24,
        [Display(Name = "تصویر شرکت بیمه (64px)", Prompt = "CompanyLogo", Description = "64*64")]
        CompanyLogo64 = 25,
        [Display(Name = "تصاویر یادآوری", Prompt = "ProposalFilledFormReminder", Description = "1000*1000")]
        ProposalFilledFormReminder = 26,
        [Display(Name = "متن قرارداد", Prompt = "ProposalFormContract")]
        ProposalFormContractRules = 27,
        [Display(Name = "لوگوی سایت متنی", Prompt = "MainLogo", Description = "300*113", AutoGenerateFilter = true)]
        MainLogoWithText = 28,
        [Display(Name = "متن قرارداد ثبت نام کاربر", Prompt = "UserRegisterFormRules")]
        UserRegisterFormRules = 29,
        [Display(Name = "نمونه اسنداد مورد نیاز ثبت نام", Prompt = "RegisterDownloadSampleDocuments")]
        RegisterDownloadSampleDocuments = 30,
        [Display(Name = "اسناد بارگزاری شده توسط کاربر جهت ثبت نام", Prompt = "RegisterUploadedDocuments", Description = "~1000*1000")]
        RegisterUploadedDocuments = 31,
        [Display(Name = "مشتریان ما یا شرکت های طرف قرارداد", Prompt = "OurObject", Description = "300*300")]
        OurObject = 32,
        [Display(Name = "مدارک نمونه فایل های انتخابی ثبت فرم تفاهم نامه", Prompt = "ContractUploadFileSample")]
        ContractUploadFileSample = 33,
        [Display(Name = "مدارک فرم پیشنهاد قرارداد", Prompt = "InsuranceContractProposalFilledForm", Description = "~1000*1000")]
        InsuranceContractProposalFilledForm = 34,
        [Display(Name = "مدارک تیکت", Prompt = "TicketUser")]
        TicketUser = 35,
        [Display(Name = "فایل های چت آنلاین", Prompt = "OnlineFile")]
        OnlineFile = 36,
        [Display(Name = "شرایط عمومی مناقصه", Prompt = "TenderGeneralLow")]
        TenderGeneralLow = 37,
        [Display(Name = "تعیین قیمت مناقصه", Prompt = "TenderPrice")]
        TenderPrice = 38,
        [Display(Name = "صدور مناقصه", Prompt = "IssueTender")]
        IssueTender = 39,
        [Display(Name = "پیام های کاربر", Prompt = "UserMessageFile")]
        UserMessageFile = 40,
        [Display(Name = "مدرک صدور فرم پیشنهاد", Prompt = "IssueProposalForm")]
        IssueProposalForm = 41,
        [Display(Name = "پس زمینه صفحه لاگین", Description = "~2000*2000")]
        LoginBackground = 42,
        [Display(Name = "تصویر اسلایدر صفحه", Prompt = "PageSlider", Description = "1900*380", AutoGenerateField = false)]
        PageSlider = 43,
        [Display(Name = "تصویر پرداخت کارت به کارت", Prompt = "UserRigisterPaymentFile", Description = "1000*1000")]
        UserRigisterPaymentFile = 44,
        [Display(Name = "مدارک تغییر وضعیت فرم پیشنهاد", Prompt = "ProposalFilledFormLogFile")]
        ProposalFilledFormLogFile = 45,
        [Display(Name = "فایل پیش فرض مدارک مورد نیاز فرم عمومی", Prompt = "GeneralForm", Description = "300*100")]
        GeneralForm = 46,
        [Display(Name = "مدارک فرم عمومی", Prompt = "GeneralFilledForm", Description = "~1000*1000", AutoGenerateFilter = true)]
        GeneralFilledForm = 47
    }
}
