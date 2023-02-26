using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum YesOrNo
    {
        [Display(Name = "خیر", Prompt = "false")]
        No = 0,
        [Display(Name = "بلی", Prompt = "true")]
        Yes = 1
    }
}
