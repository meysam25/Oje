using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum HaveOrDontHave
    {
        [Display(Name = "ندارم", Prompt = "false")]
        No = 0,
        [Display(Name = "دارم", Prompt = "true")]
        Yes = 1
    }
}
