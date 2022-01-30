using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BlockAutoIpAction: byte
    {
        [Display(Name = "قبل از اجرا")]
        BeforeExecute = 1,
        [Display(Name = "بعد از اجرا")]
        AfterExecute = 2
    }
}
