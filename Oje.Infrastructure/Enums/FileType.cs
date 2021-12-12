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
    }
}
