using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum PropertyInputType
    {
        [Display(Name = "متن")]
        Text = 1,
        [Display(Name = "فایل")]
        File = 2
    }
}
