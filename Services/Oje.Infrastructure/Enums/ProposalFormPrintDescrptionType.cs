using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum ProposalFormPrintDescrptionType : byte
    {
        [Display(Name = "هدر")]
        Header = 1,
        [Display(Name = "فوتر")]
        Footer = 2
    }
}
