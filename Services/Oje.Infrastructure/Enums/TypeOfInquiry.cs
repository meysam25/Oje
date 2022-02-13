using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum TypeOfInquiry
    {
        [Display(Name = "با شماره پلاک")]
        WithPlaque = 1,
        [Display(Name = "بدون شماره پلاک")]
        widthOutPlaque = 2
    }
}
