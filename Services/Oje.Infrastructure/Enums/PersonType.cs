using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum PersonType
    {
        [Display(Name = "حقیقی")]
        Real = 1,
        [Display(Name = "حقوقی")]
        Legal = 2
    }
}
