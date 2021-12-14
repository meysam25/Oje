using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum CreditStatus
    {
        [Display(Name = "سر رسید نشده")]
        NotYet = 1,
        [Display(Name = "سر رسید شده")]
        ItsTimeKnow = 2,
        [Display(Name = "سر رسید گزشته")]
        ItsTimePass = 3,
        [Display(Name = "وصول شده")]
        GetMony = 4
    }
}
