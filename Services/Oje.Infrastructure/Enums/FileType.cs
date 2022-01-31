using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Display(Name = "مدارک فرم پیشنهاد", Prompt = "ProposalFilledForm", Description = "~1000*1000")]
        ProposalFilledForm = 10,
        [Display(Name ="مدارک تایین قیمت شرکت", Prompt = "ProposalFilledFormCompanies")]
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
        [Display(Name = "تصویر اصلی صفحه", Prompt = "Page", Description = "1500*300", AutoGenerateField = true)]
        PageMainImage = 17,
        [Display(Name = "تصویر اصلی صفحه (کوچک)", Prompt = "Page", Description = "800*600", AutoGenerateField = true)]
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
    }
}
