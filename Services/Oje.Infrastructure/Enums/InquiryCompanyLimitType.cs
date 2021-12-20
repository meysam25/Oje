using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum InquiryCompanyLimitType
    {
        [Display(Name = "ثالث")]
        ThirdParty = 1,
        [Display(Name = "بدنه")]
        CarBody = 2,
        [Display(Name = "آتش سوزی")]
        Fire = 3
    }
}
