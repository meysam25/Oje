using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BlogTypes
    {
        [Display(Name = "متن")]
        Text = 1,
        [Display(Name = "صدا")]
        Sound = 2,
        [Display(Name = "ویدئو")]
        Video = 3
    }
}
