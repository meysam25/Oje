using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BlogSortTypes
    {
        [Display(Name ="جدید")]
        New = 1,
        [Display(Name = "پربازدید ترین")]
        Viewed = 2,
        [Display(Name = "پر دیدگاه ترین")]
        Commented = 3
    }
}
