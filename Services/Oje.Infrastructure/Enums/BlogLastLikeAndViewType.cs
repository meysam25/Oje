using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BlogLastLikeAndViewType
    {
        [Display(Name = "مشاهده")]
        View = 1,
        [Display(Name = "پسندید")]
        Like = 2
    }
}
