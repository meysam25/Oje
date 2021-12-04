using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum FireDangerGroupLevelType
    {
        [Display(Name = "سطح یک")]
        Level1 = 1,
        [Display(Name = "سطح دو")]
        Level2 = 2,
        [Display(Name = "سطح سه")]
        Level3 = 3,
        [Display(Name = "سطح چهار")]
        Level4 = 4,
        [Display(Name = "سطح پنج")]
        Level5 = 5,
        [Display(Name = "سطح شش")]
        Level6 = 6,
        [Display(Name = "سطح هفت")]
        Level7 = 7,
        [Display(Name = "سطح هشت")]
        Level8 = 8
    }
}
