using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum IsActive
    {
        [Display(Name = "فعال", Prompt = "true")]
        Active = 1,
        [Display(Name = "غیر فعال", Prompt = "false")]
        InActive = 0
    }
}
