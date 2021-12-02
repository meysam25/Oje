using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
