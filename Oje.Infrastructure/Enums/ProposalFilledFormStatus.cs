using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum ProposalFilledFormStatus
    {
        [Display(Name = "جدید")]
        New = 1,
        [Display(Name = "در انتظار تایید")]
        W8ForConfirm = 2,
        [Display(Name = "نیاز به کارشناس")]
        NeedSpecialist = 3,
        [Display(Name = "تایید شده")]
        Confirm = 4,
        [Display(Name = "صادر شده")]
        Issuing = 5,
        [Display(Name = "رد شده")]
        NotIssue = 6
    }
}
