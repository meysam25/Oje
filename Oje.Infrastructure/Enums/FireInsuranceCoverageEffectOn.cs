using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum FireInsuranceCoverageEffectOn
    {
        [Display(Name = "سرمایه")]
        Investment = 1,
        [Display(Name = "ارزش اثاثیه")]
        AssetsValue = 2,
        [Display(Name = "ارزش ساختمان")]
        ValueOfBuilding = 3,
        [Display(Name = "توسط کاربر وارد می شود")]
        InputByUser = 4
    }
}
