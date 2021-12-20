using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum RoundInqueryType
    {
        [Display(Name = "پایین")]
        Down = 1,
        [Display(Name = "بالا")]
        Up = 2,
        [Display(Name = "بالا یا پایین")]
        DownUp = 3
    }
}
