using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum SalesNetworkType
    {
        [Display(Name = "معمولی")]
        Normal = 1,
        [Display(Name = "چند سطحی")]
        Multilevel = 2
    }
}
