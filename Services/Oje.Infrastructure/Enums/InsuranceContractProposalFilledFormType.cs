using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum InsuranceContractProposalFilledFormType
    {
        [Display(Name = "مدارک جدید")]
        New = 1,
        [Display(Name = "در حال برسی")]
        Pending = 2,
        [Display(Name = "نقص مدارک")]
        DefectiveDocument = 3,
        [Display(Name = "اصلاح شده")]
        Reviewed = 4,
        [Display(Name = "تایید شده")]
        Confirm = 5,
        [Display(Name = "در انتظار دریافت اصل مدارک")]
        W8ForReciveingRealDocuments = 6,
        [Display(Name = "رد شده")]
        Rejected = 7,
        [Display(Name = "پرداخت شده")]
        Payed = 8
    }
}
